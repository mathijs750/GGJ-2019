using Controllers;
using Interfaces;
using ScriptableObjects;
using UnityEngine;

namespace Drops
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class BasicBlock : MonoBehaviour, IDropable
    {
        private Rigidbody2D _rb;
        public PlayerBirdController PlayerRef { get; set; }
        private bool _isAttached;
        private GameEvent _onDoneEvent;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
            //_rb.isKinematic = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"AUW ik raak {other.gameObject.name}");
            if (_isAttached) EnableDropping(); 
            else if (_onDoneEvent != null) _onDoneEvent.Raise();
        }

        public void EnableDropping()
        {
            _isAttached = false;
            //_rb.isKinematic = false;
        }

        public void AttachToBird()
        {
            if (_rb != null) _rb.isKinematic = false;
            _isAttached = true;
        }

        public void MakeLast(GameEvent endEvent)
        {
            _onDoneEvent = endEvent;
        }
    }
}
