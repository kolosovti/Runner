using System.Collections;
using System.Collections.Generic;
using Game.Core.Controllers;
using Game.Core.Road;
using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force;

        private PlayerController _playerController;
        private BaseRoadObject _baseRoadObject;

        public Rigidbody Rigidbody => _rigidbody;

        public void SetController(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up * _force);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //TODO: extract tag to global variables
            if (collision.gameObject.CompareTag("Road"))
            {
                _baseRoadObject = collision.gameObject.GetComponent<BaseRoadObject>();
            }
        }
    }
}