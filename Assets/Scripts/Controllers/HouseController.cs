using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class HouseController : MonoBehaviour
    {
        public bool IslastInQueue;

        [SerializeField] private GameEvent _goodFeedback;
        [SerializeField] private GameEvent _badFeedback;
        [SerializeField] private GameObject[] _blocksForQueue;
        private int _currentIndex;

        public delegate void BlockQueueEmpty();

        public static event BlockQueueEmpty OnEmpty;

        public GameObject GetNextBlock()
        {
            _currentIndex++;
            if (_currentIndex > _blocksForQueue.Length - 1)
            {
                OnEmpty?.Invoke();
                GameManager.Instance.SpawnNewHouseController();
                return null;
            }

            return _blocksForQueue[_currentIndex];
        }

        private void Awake()
        {
            _currentIndex = -1;
        }
    }
}