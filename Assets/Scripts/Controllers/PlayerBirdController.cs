using System;
using Interfaces;
using Managers;
using ScriptableObjects;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Controllers
{
    public class PlayerBirdController : MonoBehaviour
    {
        [SerializeField] private GameEvent RespawnEvent;
        private HingeJoint2D _hinge;
        private IDropable _payload;
        private Rigidbody2D _rb;
        private readonly Vector2 _spawnPoint = new Vector2(0, 8);

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void DropBlock()
        {
            
            Destroy(GetComponent<HingeJoint2D>());
            _payload.EnableDropping();
        }

        public void Respawn(GameObject newDrop)
        {
            Destroy(GetComponent<HingeJoint2D>());
            transform.position = _spawnPoint;
            var newDropInstance = Instantiate(newDrop, transform.localPosition, Quaternion.identity);            
            _payload = newDrop.GetComponent<IDropable>();

            _hinge = gameObject.AddComponent<HingeJoint2D>();
            _hinge.anchor = Vector2.zero;
            _hinge.anchor = Vector2.zero;
//            _hinge.motor = new JointMotor2D()
//            {
//                maxMotorTorque = 5f,
//                motorSpeed = 0
//            };
            _hinge.connectedBody = newDropInstance.GetComponent<Rigidbody2D>();

            _payload.AttachToBird();
        }

        private void Update()
        {
            _rb.AddForce( Vector2.right * (Input.GetAxis("Horizontal") * GameManager.Instance.MovementSpeed) * 1000f);
            //transform.position += Vector3.down * GameManager.Instance.DropRate;

            if (Input.GetKeyDown(KeyCode.Space)) DropBlock();
            
            if (transform.position.y < -2f) RespawnEvent.Raise();
        }
    }
}