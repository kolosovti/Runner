using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up);
        }
    }
}
