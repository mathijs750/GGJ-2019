using System.Collections.Generic;
using Controllers;
using Interfaces;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject[] prefabs;
        public float DropRate = 0.2f;
        public float MovementSpeed = 1f;
   
        public float Progress { get; private set; }
        public int Score { get; private set; }
        public Queue<GameObject> BlockList { get; private set; }

        public PlayerBirdController birdRef;
        
        public static GameManager Instance;

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
            Instance.SetUpQueue();
            Instance.birdRef.Respawn();
            
        }

        private void SetUpQueue()
        {
            foreach (var prefab in prefabs)
            {
                BlockList.Enqueue(Instantiate(prefab));
            }
        }
    }
}