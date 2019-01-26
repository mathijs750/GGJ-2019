using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Controllers;
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
        
        [SerializeField]
        private GameObject[] _housePlacePrefabs;


        public float DropRate = 20f;
        public float MovementSpeed = 1f;

        public float Progress { get; private set; }
        public int Score { get; private set; }

        public GameState CurrentState => _state;
        private Queue<int> BlockIndexList { get; set; }
        public static float WaitTime => 5f;

        public static GameManager Instance;        

        public void ChangeState(GameState desiredState)
        {
            var prevState = Instance._state;
            Debug.Log($"Changing state from {prevState} => {desiredState}");

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

        private IEnumerator ProgressClock(float timeLimit)
        {
            Instance.Progress += 0.5f;
            yield return new WaitForSeconds(0.5f);
//            if ()
//            StartCoroutine()
        }

        private IEnumerator TimedStateChange(GameState newState)
        {
            yield break;
        }
    }

}