using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerBirdController : MonoBehaviour
    {
        public Transform AttachmentPoint;

        private HingeJoint2D _joint;
        private IDropable _payload;
        private readonly Vector2 _spawnPoint = new Vector2(0, 20);

        public void DropBlock()
        {
            _joint.breakForce = 0;
            _joint.breakTorque = 0;
            _payload.EnableDropping();

            
        }

        public void Respawn()
        {
            var newDrop = GameManager.Instance.BlockList.Dequeue();
            if (newDrop != null) return;
            
            transform.position = _spawnPoint;
            _payload = newDrop.GetComponent<IDropable>();
            _joint.connectedBody = newDrop.GetComponent<Rigidbody2D>();
            _joint.breakForce = float.PositiveInfinity;
            _joint.breakTorque = float.PositiveInfinity;
        }

        private void Start()
        {
            _joint = GetComponent<HingeJoint2D>();
            
        }

        private void Update()
        {
            transform.position += Vector3.right * (Input.GetAxis("Horizontal") * GameManager.Instance.MovementSpeed);
            transform.position += Vector3.down * GameManager.Instance.DropRate;
        }
    }
}