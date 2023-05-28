using System;
using UniRx;

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
        public ReactiveProperty<int> Health;
        public ReactiveProperty<int> JumpsCount;
        public ReactiveProperty<bool> IsGrounded;

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