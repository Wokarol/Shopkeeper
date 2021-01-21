using TMPro;
using UnityEngine;

namespace Shopkeeper.Mining
{
    public class FillAmountSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countLabel = null;
        [SerializeField] private RectTransform slider = null;

        public float MaxValue;
        public float CurrentValue
        {
            set
            {
                countLabel.text = $"{value} / {MaxValue}";
                Vector2 a = slider.anchorMax;
                a.x = value / MaxValue;
                slider.anchorMax = a;
            }
        }
    }
}
