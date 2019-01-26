using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class HouseController : MonoBehaviour
    {
        [SerializeField] private GameEvent _goodFeedback;
        [SerializeField] private GameEvent _badFeedback;
        [SerializeField] private GameObject[] _blocksForQueue;
        private int _currentIndex;

        private void Awake()
        {
            _currentIndex = -1;
        }

        private (GameObject, bool) GetNextBlock()
        {
            _currentIndex++;
            return _currentIndex == _blocksForQueue.Length - 1
                ? (_blocksForQueue[_currentIndex], true)
                : (_blocksForQueue[_currentIndex], false);
        }
        
    }
}