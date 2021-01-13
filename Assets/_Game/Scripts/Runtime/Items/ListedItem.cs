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
        public List<Item> Items;
        public int Amount;

        public Item Item => Items[0];

        public VisibleItemStack(Item item, int amount)
        {
            Items = new List<Item> { item };
            Amount = amount;
        }
    }

    public class ListedItem : MonoBehaviour
    {
        [Header("Icon, should have disabled shadow")]
        [SerializeField] private Image icon;
        [SerializeField] private float spacing;
        [Header("Text")]
        [SerializeField] private TMPro.TextMeshProUGUI count;
        [Header("Style - Normal")]
        [SerializeField] private Color normalColor = Color.white;
        [Header("Style - Empty")]
        [SerializeField] private Material emptyMaterial = null;
        [SerializeField] private Color emptyColor = Color.black;


        public VisibleItemStack Stack { get; private set; }

        private List<DisplayedIcon> icons = new List<DisplayedIcon>();

        private void Start()
        {
            icons.Add(new DisplayedIcon(icon));
        }

        public void Set(VisibleItemStack stack)
        {
            Stack = stack;
            count.text = stack.Amount.ToString();

            if (stack.Amount == 0)
            {
                icon.sprite = stack.Items[0].Sprite;
                icon.material = emptyMaterial;
                icon.color = emptyColor;
            }
            else
            {
                EnsureIconCount(stack.Items.Count);
                for (int i = 0; i < stack.Items.Count; i++)
                {
                    DisplayedIcon displayedIcon = icons[i];
                    Item item = stack.Items[i];

                    displayedIcon.Icon.sprite = item.Sprite;
                    displayedIcon.Shadow.enabled = i != 0;

                    displayedIcon.Icon.material = icon.defaultMaterial;
                    displayedIcon.Icon.color = normalColor;
                }
            }
        }

        private void EnsureIconCount(int count)
        {
            if (icons.Count < count)
            {
                int toAdd = count - icons.Count;
                for (int i = 0; i < toAdd; i++)
                {
                    Image newIcon = Instantiate(icon, icon.transform.parent);
                    icons.Add(new DisplayedIcon(newIcon));
                }

                float offset = (count - 1) / 2f;
                for (int i = 0; i < count; i++)
                {
                    DisplayedIcon icon = icons[i];
                    icon.RectTransform.anchoredPosition = new Vector2((i - offset) * spacing, 0);
                }
            }
        }

        internal void GetIconPlacing(int index, out Vector2 position, out Vector2 size)
        {
            position = icons[index].RectTransform.position;
            size = Vector2.one * 85;
        }

        struct DisplayedIcon
        {
            public Image Icon;
            public Shadow Shadow;
            public RectTransform RectTransform;

            public DisplayedIcon(Image icon)
            {
                Icon = icon;
                Shadow = icon.GetComponent<Shadow>();
                RectTransform = icon.transform as RectTransform;
            }
        }
    }
}
