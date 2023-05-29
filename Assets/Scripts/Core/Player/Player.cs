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

        private PlayerController _playerController;

        public Rigidbody Rigidbody => _rigidbody;

        public void SetController(PlayerController playerController)
        {
            _playerController = playerController;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Road"))
            {
                _playerController.PlayerGrounded();
            }
        }
    }
}