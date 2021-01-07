using System;
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
        [SerializeField] private RectTransform posTarget;
        [Space]
        [SerializeField] private Color normalColor = Color.white;

        [SerializeField] private Material emptyMaterial = null;
        [SerializeField] private Color emptyColor = Color.black;
        public ItemStack Stack { get; private set; }

        public void Set(ItemStack stack)
        {
            Stack = stack;
            icon.sprite = stack.Item.Sprite;
            count.text = stack.Amount.ToString();

            bool empty = stack.Amount == 0;
            icon.material = empty ? emptyMaterial : icon.defaultMaterial;
            icon.color = empty ? emptyColor : normalColor;
        }

        internal void GetIconPlacing(out Vector3 position, out Vector2 size)
        {
            position = posTarget.position;
            size = Vector2.one * 85;
        }
    }
}
