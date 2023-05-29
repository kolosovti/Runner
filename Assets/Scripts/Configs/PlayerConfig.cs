using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _jumpForce;
        [SerializeField] private int _maxJumpCount;

        public Vector3 JumpForce => _jumpForce;
        public int MaxJumpCount => _maxJumpCount;
    }
}
