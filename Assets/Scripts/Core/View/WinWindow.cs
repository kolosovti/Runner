using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Window
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _nextLevel;
        [SerializeField] private StatsView _statsView;

        public IObservable<Unit> OnNextLevelButtonClick => _nextLevel.OnClickAsObservable();

        public void Show(StatsData data)
        {
            _statsView.Set(data);
        }
    }
}