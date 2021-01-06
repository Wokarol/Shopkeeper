using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    public class ListedItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMPro.TextMeshProUGUI count;
        [Space]
        [SerializeField] private Color normalColor = Color.white;

        [SerializeField] private Material emptyMaterial = null;
        [SerializeField] private Color emptyColor = Color.black;

        public void Set(ItemStack stack)
        {
            icon.sprite = stack.Item.Sprite;
            count.text = stack.Amount.ToString();

            bool empty = stack.Amount == 0;
            icon.material = empty ? emptyMaterial : icon.defaultMaterial;
            icon.color = empty ? emptyColor : normalColor;
        }
    }
}
