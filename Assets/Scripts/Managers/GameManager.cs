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
        private GameState _state = GameState.StartUp;
        private HouseController _houseCon;

        [SerializeField] private GameObject[] _housePlacePrefabs;
        private int _houseIndex;

        private bool _canChangeState;

        public float DropRate = 20f;
        public float MovementSpeed = 1f;

        public float Progress { get; private set; }
        public int Score { get; private set; }

        public GameState CurrentState => _state;
        public HouseController CurrentHouseController => _houseCon;
        
        public static float WaitTime => 5f;

        [FormerlySerializedAs("StartGameLoopEvent")] [SerializeField] private GameEvent _startGameLoopEvent;

        public static GameManager Instance;

        public void SpawnNewHouseController()
        {
            _houseIndex++;
            var house = Instantiate(_housePlacePrefabs[_houseIndex], transform.localPosition, Quaternion.identity);
            _houseCon = house.GetComponent<HouseController>();
            _startGameLoopEvent.Raise();
        }

        public void ChangeState(GameState desiredState)
        {
            if (!_canChangeState) return;
            var prevState = Instance._state;
            Debug.Log($"Changing state from {prevState} => {desiredState}");

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
            _houseCon = null;
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
            var prevState = _state;

            switch (newState)
            {
                case GameState.StartUp:
                    UiController.Instance.FadeToBlack();
                    break;
                case GameState.SplashScreen:
                    UiController.Instance.ShowSplash();
                    yield return new WaitForSeconds(3);
                    UiController.Instance.HideSplash();
                    yield return new WaitForSeconds(3);
                    UiController.Instance.ShowInfo();
                    yield return new WaitForSeconds(10);
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
                    UiController.Instance.FadeToBlack();
                    yield return new WaitForSeconds(3);
                    UiController.Instance.ShowScore();
                    yield return new WaitForSeconds(3);

                    break;
                default:
                    yield break;
            }

            _state = newState; 
            _canChangeState = true;
        }
    }
}