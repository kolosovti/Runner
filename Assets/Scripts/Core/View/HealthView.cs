using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        public void Set(float health)
        {
            health = health < 0 ? 0f : health;
            _healthText.text = health.ToString("F0");
        }
    }
}
