using System.Collections;
using Controllers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public enum GameState
    {
        StartUp,
        SplashScreen,
        GamePlay,
        ScoreReview
    }

    public class GameManager : MonoBehaviour
    {
        public float DropRate = 20f;
        public float MovementSpeed = 1f;
        public static GameManager Instance;

        [SerializeField] private GameObject[] _housePlacePrefabs;
        private int _houseIndex;
        private bool _canChangeState;

        [SerializeField]
        private GameEvent _startGameLoopEvent;
        [SerializeField]
        private GameEvent _endGameLoopEvent;

        public float Progress { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.StartUp;
        public HouseController CurrentHouseController { get; private set; }
        public static float WaitTime => 5f;
        public int Score
        {
            get { return 0; }
            set { throw new System.NotImplementedException(); }
        }

        public void SpawnNewHouseController()
        {            
            _houseIndex++;
            if (_houseIndex >= _housePlacePrefabs.Length)
            {
                ChangeState(GameState.ScoreReview);
                return;
            }
            Destroy(CurrentHouseController);
            var house = Instantiate(_housePlacePrefabs[_houseIndex], transform.localPosition, Quaternion.identity);
            CurrentHouseController = house.GetComponent<HouseController>();
            _startGameLoopEvent.Raise();
        }

        public void ChangeState(GameState desiredState)
        {
            if (!_canChangeState) return;
            Debug.Log($"Changing state from {Instance.CurrentState} => {desiredState}");

            StartCoroutine(TimedStateChange(desiredState));
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _canChangeState = true;
            ChangeState(GameState.SplashScreen);
            StartCoroutine($"ProgressClock");
            _houseIndex = -1;
            CurrentHouseController = null;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit(1);
        }

        private IEnumerator ProgressClock()
        {
            Instance.Progress += 0.5f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine($"ProgressClock");
        }

        private IEnumerator TimedStateChange(GameState newState)
        {
            _canChangeState = false;
            var prevState = CurrentState;

            #if UNITY_EDITOR
            //newState = GameState.GamePlay;
            #endif

            switch (newState)
            {
                case GameState.StartUp:
                    UiController.Instance.FadeToBlack();
                    break;
                case GameState.SplashScreen:
                    UiController.Instance.ShowSplash();
                    yield return new WaitForSeconds(2);
                    UiController.Instance.HideSplash();
                    yield return new WaitForSeconds(2);
                    UiController.Instance.ShowInfo();
                    yield return new WaitForSeconds(7);
                    UiController.Instance.HideInfo();
                    yield return new WaitForSeconds(2);
                    StartCoroutine(TimedStateChange(GameState.GamePlay));
                    yield break;

                case GameState.GamePlay:
                    UiController.Instance.FadeToGame();
                    yield return new WaitForSeconds(3);
                    SpawnNewHouseController();
                    break;
                case GameState.ScoreReview:
                    _endGameLoopEvent.Raise();
                    UiController.Instance.FadeToBlack();
                    yield return new WaitForSeconds(3);
                    UiController.Instance.ShowScore();
                    yield return new WaitForSeconds(10);
                    
                    yield break;
                default:
                    yield break;
            }

            CurrentState = newState;
            _canChangeState = true;
        }
    }
}