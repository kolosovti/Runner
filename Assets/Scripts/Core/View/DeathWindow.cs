using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Window
{
    public class DeathWindow : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private StatsView _statsView;

        public IObservable<Unit> OnNextLevelButtonClick => _nextLevelButton.OnClickAsObservable();
        public IObservable<Unit> OnReviveButtonClick => _reviveButton.OnClickAsObservable();

        public void Show(StatsData data)
        {
            _statsView.Set(data);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}