using Game.Core.Controllers;
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
            if (other.CompareTag(Tags.Road))
            {
                _playerController.PlayerGrounded();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Road))
            {
                _playerController.PlayerFall();
            }
        }
    }
}