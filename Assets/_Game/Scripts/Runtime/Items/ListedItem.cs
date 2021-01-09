using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    [System.Serializable]
    public struct VisibleItemStack
    {
        public Item Item;
        public int Amount;

        public VisibleItemStack(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    public class ListedItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMPro.TextMeshProUGUI count;
        [SerializeField] private RectTransform posTarget;
        [Space]
        [SerializeField] private Color normalColor = Color.white;

        [SerializeField] private Material emptyMaterial = null;
        [SerializeField] private Color emptyColor = Color.black;
        public VisibleItemStack Stack { get; private set; }

        public void Set(VisibleItemStack stack)
        {
            Stack = stack;
            icon.sprite = stack.Item.Sprite;
            count.text = stack.Amount.ToString();

            bool empty = stack.Amount == 0;
            icon.material = empty ? emptyMaterial : icon.defaultMaterial;
            icon.color = empty ? emptyColor : normalColor;
        }

        internal void GetIconPlacing(int index, out Vector2 position, out Vector2 size)
        {
            // TODO: Implement inner index
            position = posTarget.position;
            size = Vector2.one * 85;
        }
    }
}
