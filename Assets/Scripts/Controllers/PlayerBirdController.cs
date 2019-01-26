using System;
using Interfaces;
using Managers;
using ScriptableObjects;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class PlayerBirdController : MonoBehaviour
    {
        [SerializeField] private GameEvent RespawnEvent;

        public float MovementForce = 1000f;

        private HingeJoint2D _hinge;
        private IDropable _payload;
        private Rigidbody2D _rb;
        private readonly Vector2 _spawnPoint = new Vector2(0, 8);


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = GameManager.Instance.DropRate;

            Debug.Log($"Drag is {_rb.drag}");
        }

        public void DropBlock()
        {
            Destroy(GetComponent<HingeJoint2D>());
            _payload.EnableDropping();
            Invoke($"RequestRespawn", GameManager.WaitTime);
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
            _hinge.motor = new JointMotor2D()
            {
                maxMotorTorque = 0.01f,
                motorSpeed = 0
            };
            _hinge.connectedBody = newDropInstance.GetComponent<Rigidbody2D>();

            _payload.AttachToBird();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.GamePlay) return;

            _rb.AddForce(Vector2.right * (Input.GetAxis("Horizontal") * GameManager.Instance.MovementSpeed) *
                         MovementForce);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                _rb.AddForce(Vector2.up * MovementForce / 3f);
            if (Input.GetKeyDown(KeyCode.Space))
                DropBlock();

            if (!(transform.position.y < -2f)) return;
            CancelInvoke($"RequestRespawn");
            RespawnEvent.Raise();
        }

        private void RequestRespawn()
        {
            RespawnEvent.Raise();
        }
    }
}