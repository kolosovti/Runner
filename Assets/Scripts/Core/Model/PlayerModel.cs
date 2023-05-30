using System;
using UniRx;
using UnityEngine;

namespace Game.Core.Model
{
    public interface IPlayerModel
    {
        Player Player { get; }
        IReadOnlyReactiveProperty<int> Health { get; }
        IReadOnlyReactiveProperty<int> JumpsCount { get; }
        IReadOnlyReactiveProperty<bool> IsGrounded { get; }
    }

    //TODO: передавать ассет референс на префаб игрока, чтобы PlayerController мог подзагружать только нужный скин игрока из аддрессаблов
    public class PlayerModel : IPlayerModel
    {
        public Player Player;
        public ReactiveProperty<int> Health = new ReactiveProperty<int>();
        public ReactiveProperty<int> JumpsCount = new ReactiveProperty<int>();
        public ReactiveProperty<bool> IsGrounded = new ReactiveProperty<bool>();

        Player IPlayerModel.Player => Player;
        IReadOnlyReactiveProperty<int> IPlayerModel.Health => Health;
        IReadOnlyReactiveProperty<int> IPlayerModel.JumpsCount => JumpsCount;
        IReadOnlyReactiveProperty<bool> IPlayerModel.IsGrounded => IsGrounded;

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void OnJump()
        {
            JumpsCount.Value++;
            IsGrounded.Value = false;
        }
    }
}