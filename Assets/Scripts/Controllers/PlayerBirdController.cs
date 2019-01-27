using Interfaces;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Controllers
{
    public class PlayerBirdController : MonoBehaviour
    {
        [SerializeField] private GameEvent RespawnEvent;
        [SerializeField] private GameEvent LastEvent;
        private bool _controlsEnabled;

        public float MovementForce = 800f;

        private HingeJoint2D _hinge;
        private IDropable _payload;
        private Rigidbody2D _rb;
        private bool _upIsPressed;
        private readonly Vector2 _spawnPoint = new Vector2(-1, 13);


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.drag = GameManager.Instance.DropRate;
            _rb.isKinematic = true;
        }

        public void EnableControls()
        {
            _rb.isKinematic = false;
            _upIsPressed = false;
            RequestRespawn();
        }
        
        public void DisableControls()
        {
            _rb.isKinematic = true;
        }
        
        public void DropBlock()
        {
            Destroy(GetComponent<HingeJoint2D>());
            _rb.drag = GameManager.Instance.DropRate / 3;
            _payload.EnableDropping();
            Invoke($"RequestRespawn", GameManager.WaitTime);
        }

        public void Respawn(GameObject newDrop, bool isLast)
        {
            if (newDrop == null) return;
            Destroy(GetComponent<HingeJoint2D>());
            transform.position = _spawnPoint;
            var newDropInstance = Instantiate(newDrop, transform.localPosition, Quaternion.identity);
            _payload = newDrop.GetComponent<IDropable>();
            _rb.drag = GameManager.Instance.DropRate;

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
//            Debug.Log($"{GameManager.Instance.CurrentState}");
//            if (Input.GetButtonDown("Jump") && GameManager.Instance.CurrentState == GameState.InfoScreen)
//            {
//                GameManager.Instance.ChangeState(GameState.GamePlay);
//                Debug.Log("E!");
//                EnableControls();
//            } 
            if (GameManager.Instance.CurrentState != GameState.GamePlay) return;

            _rb.AddForce(Vector2.right * (Input.GetAxis("Horizontal") * GameManager.Instance.MovementSpeed) *
                         MovementForce);

            if (Input.GetAxisRaw("Vertical") > 0.6f && !_upIsPressed)
            {
                _rb.AddForce(Vector2.up * MovementForce / 2f);
                _upIsPressed = true;
            }
            else if (Input.GetAxisRaw("Vertical") < 0.3f && _upIsPressed)
            {
                _upIsPressed = false;
            }
               
            if (Input.GetButtonDown("Jump"))
                DropBlock();

            if (!(transform.position.y < -2f)) return;
            CancelInvoke($"RequestRespawn");
            RequestRespawn();
        }

        private void RequestRespawn()
        {
            RespawnEvent.Raise();
            var nextHouse = GameManager.Instance.CurrentHouseController.GetNextBlock();
            var isLastHouse = GameManager.Instance.CurrentHouseController.IslastInQueue;
            Respawn(nextHouse, isLastHouse);
        }
        
        
    }
}