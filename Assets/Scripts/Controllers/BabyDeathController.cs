using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class BabyDeathController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _particles;

        [SerializeField] private GameEvent _killEvent;

        public void AddScore()
        {
            GameManager.Instance.Score += 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _killEvent.Raise();
            Instantiate(_particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}