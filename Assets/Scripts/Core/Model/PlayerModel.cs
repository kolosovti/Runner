using System;
using UniRx;
using UnityEngine;

namespace Game.Core.Model
{
    public interface IPlayerModel
    {
        Player Player { get; }
        IObservable<int> Health { get; }
        IObservable<int> JumpsCount { get; }
        IObservable<bool> IsGrounded { get; }
    }

    //TODO: передавать ассет референс на префаб игрока, чтобы PlayerController мог подзагружать только нужный скин игрока из аддрессаблов
    public class PlayerModel : IPlayerModel
    {
        public Player Player;
        public ReactiveProperty<int> Health = new ReactiveProperty<int>();
        public ReactiveProperty<int> JumpsCount = new ReactiveProperty<int>();
        public ReactiveProperty<bool> IsGrounded = new ReactiveProperty<bool>();

        Player IPlayerModel.Player => Player;
        IObservable<int> IPlayerModel.Health => Health;
        IObservable<int> IPlayerModel.JumpsCount => JumpsCount;
        IObservable<bool> IPlayerModel.IsGrounded => IsGrounded;

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void OnJump()
        {
            JumpsCount.Value++;
            IsGrounded.Value = false;
        }

        public void OnGrounded()
        {
            JumpsCount.Value = 0;
            IsGrounded.Value = true;
        }
    }
}