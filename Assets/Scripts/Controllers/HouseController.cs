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

        public GameObject GetNextBlock()
        {
            _currentIndex++;
            return _currentIndex >= _blocksForQueue.Length ? null : _blocksForQueue[_currentIndex];
        }

        private void Awake()
        {
            _currentIndex = -1;
        }
    }
}