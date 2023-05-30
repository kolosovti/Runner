using TMPro;
using UnityEngine;

namespace Game.UI.Window
{
    public class StatsData
    {
        public float HolePassedCount;
        public float LongHolePassedCount;
        public float SawPassedCount;
        public float FencePassedCount;
    }

    public class StatsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _holePassedCountText;
        [SerializeField] private TextMeshProUGUI _longHolePassedCountText;
        [SerializeField] private TextMeshProUGUI _sawPassedCountText;
        [SerializeField] private TextMeshProUGUI _fencePassedCountText;

        public void Set(StatsData data)
        {
            _holePassedCountText.text = data.HolePassedCount.ToString("F0");
            _longHolePassedCountText.text = data.LongHolePassedCount.ToString("F0");
            _sawPassedCountText.text = data.SawPassedCount.ToString("F0");
            _fencePassedCountText.text = data.FencePassedCount.ToString("F0");
        }
    }
}
