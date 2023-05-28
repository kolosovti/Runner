using System;
using UniRx;

namespace Game.Core.Model
{
    public interface IPlayerModel
    {
        IObservable<int> Health { get; }
        Player Player { get; }
    }

    //TODO: ���������� ����� �������� �� ������ ������, ����� PlayerController ��� ������������ ������ ������ ���� ������ �� ������������
    public class PlayerModel : IPlayerModel
    {
        private ReactiveProperty<int> _health;
        private Player _player;

        IObservable<int> IPlayerModel.Health => _health;
        Player IPlayerModel.Player => _player;

        public void SetPlayer(Player player)
        {
            _player = player;
        }
    }
}