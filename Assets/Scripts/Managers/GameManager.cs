using System.Collections.Generic;
using Controllers;
using Drops;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("prefabs")]
        private GameObject[] BlockPrefabKinds;

        public Sprite[] PossibleShapes;

        public float DropRate = 0.2f;
        public float MovementSpeed = 1f;

        public float Progress { get; private set; }
        public int Score { get; private set; }

        private GameObject NextBlock => BlockPrefabKinds[BlockIndexList.Dequeue()];

        private Queue<int> BlockIndexList { get; set; }

        public PlayerBirdController birdRef;

        public static GameManager Instance;

        public void RespawnBird()
        {
            birdRef.Respawn(NextBlock);
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