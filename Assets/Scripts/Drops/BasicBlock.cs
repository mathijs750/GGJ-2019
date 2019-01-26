using Interfaces;
using UnityEngine;

namespace Drops
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class BasicBlock : MonoBehaviour, IDropable
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            //_rb.isKinematic = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //Debug.Log($"AUW ik raak {other.gameObject.name}");
        }

        public void EnableDropping()
        {
            //_rb.isKinematic = false;
        }

        public void AttachToBird()
        {
            if (_rb != null) _rb.isKinematic = false;
        }
    }
}
