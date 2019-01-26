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
        [SerializeField] private GameEvent LastEvent;
        private bool controlsEnabled;

        public float MovementForce = 1000f;

        private HingeJoint2D _hinge;
        private IDropable _payload;
        private Rigidbody2D _rb;
        private readonly Vector2 _spawnPoint = new Vector2(0, 8);


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = GameManager.Instance.DropRate;
            _rb.isKinematic = true;

            Debug.Log($"Drag is {_rb.drag} because Gm is {GameManager.Instance.DropRate}");
        }

        public void EnableControls()
        {
            _rb.isKinematic = false;
            RequestRespawn();
        }
        
        public void DropBlock()
        {
            Destroy(GetComponent<HingeJoint2D>());
            _payload.EnableDropping();
            Invoke($"RequestRespawn", GameManager.WaitTime);
        }

        public void Respawn(GameObject newDrop, bool isLast)
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
            
            if (isLast) _payload.MakeLast(LastEvent);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Jump") && GameManager.Instance.CurrentState == GameState.InfoScreen)
            {
                GameManager.Instance.ChangeState(GameState.GamePlay);
                Debug.Log("E!");
                EnableControls();
            } 
            if (GameManager.Instance.CurrentState != GameState.GamePlay) return;

            _rb.AddForce(Vector2.right * (Input.GetAxis("Horizontal") * GameManager.Instance.MovementSpeed) *
                         MovementForce);

            if (Input.GetButtonDown("Fire2"))
                _rb.AddForce(Vector2.up * MovementForce / 3f);
            
            if (Input.GetButtonDown("Fire1"))
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