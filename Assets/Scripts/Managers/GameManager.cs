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
        
        [SerializeField, FormerlySerializedAs("prefabs")]
        private GameObject[] BlockPrefabKinds;

        public Sprite[] PossibleShapes;

        public float DropRate = 20f;
        public float MovementSpeed = 1f;

        public float Progress { get; private set; }
        public int Score { get; private set; }

        private GameObject NextBlock => BlockPrefabKinds[BlockIndexList.Dequeue()];

        public GameState CurrentState => _state;
        private Queue<int> BlockIndexList { get; set; }
        public static float WaitTime => 5f;

        [FormerlySerializedAs("birdRef")] public PlayerBirdController BirdRef;

        public static GameManager Instance;

        public void RespawnBird()
        {
            BirdRef.Respawn(NextBlock);
        }

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

        private void Start()
        {
            Instance.SetUpQueue(10);
            RespawnBird();
        }

        private void SetUpQueue(int size)
        {
            if (BlockIndexList == null) BlockIndexList = new Queue<int>(size+(size/2));
            
            for (int i = 0; i < size; i++)
            {
                BlockIndexList.Enqueue(Random.Range(0, BlockPrefabKinds.Length - 1));
            }
        }

        private IEnumerator ProgressClock(float timeLimit)
        {
            Instance.Progress += 0.5f;
            yield return new WaitForSeconds(0.5f);
//            if ()
//            StartCoroutine()
        }
//        
//        private void GetNextBlock(int nextIndex)
//        {
//            var block = BlockPrefabKinds[nextIndex];
//            block.GetComponent<SpriteRenderer>().sprite =
//                PossibleShapes[Random.Range(0, PossibleShapes.Length - 1)];
//            block.AddComponent<BoxCollider2D>();
//        }
    }

}