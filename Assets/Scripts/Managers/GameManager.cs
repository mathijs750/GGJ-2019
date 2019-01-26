using System.Collections;
using Controllers;
using UnityEngine;

namespace Managers
{
    public enum GameState
    {
        StartUp,
        SplashScreen,
        InfoScreen,
        GamePlay,
        ScoreReview
    }


    public class GameManager : MonoBehaviour
    {
        private GameState _state = GameState.StartUp;

        [SerializeField] private GameObject[] _housePlacePrefabs;

        private bool canChangeState;

        public float DropRate = 20f;
        public float MovementSpeed = 1f;

        public float Progress { get; private set; }
        public int Score { get; private set; }

        public GameState CurrentState => _state;
        public static float WaitTime => 5f;

        public static GameManager Instance;

        public void ChangeState(GameState desiredState)
        {
            if (!canChangeState) return;

            var prevState = Instance._state;
            Debug.Log($"Changing state from {prevState} => {desiredState}");

            StartCoroutine(TimedStateChange(desiredState));
            return;

            if (desiredState == GameState.SplashScreen)
            {
                UiController.Instance.ShowSplash();
            }

            if (prevState == GameState.ScoreReview &&
                desiredState == GameState.SplashScreen)
            {
                //TODO: clear out the progress and other things
            }
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
            canChangeState = true;
            ChangeState(GameState.SplashScreen);
        }

        private IEnumerator ProgressClock(float timeLimit)
        {
            Instance.Progress += 0.5f;
            yield return new WaitForSeconds(0.5f);
//            if ()
//            StartCoroutine()
        }

        private IEnumerator TimedStateChange(GameState newState)
        {
            canChangeState = false;
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
                    Debug.Log("Start game");
                    StartCoroutine(TimedStateChange(GameState.GamePlay));
                    yield break;

                case GameState.GamePlay:
                    UiController.Instance.FadeToGame();
                    yield return new WaitForSeconds(3);

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

            canChangeState = true;
        }
    }
}