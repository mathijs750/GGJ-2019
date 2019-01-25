using Interfaces;
using UnityEngine;

namespace Drops
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class BasicBlock : MonoBehaviour, IDropable
    {
        private Rigidbody2D _rb;
        private Collider2D _coll;

        private void OnEnable()
        {
            _rb = GetComponent<Rigidbody2D>();
            _coll = GetComponent<Collider2D>();
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
    }
}
