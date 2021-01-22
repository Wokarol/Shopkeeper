using DG.Tweening;
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
        [SerializeField] private bool animation = false;
        [Header("Text")]
        [SerializeField] private TMPro.TextMeshProUGUI count;
        [Header("Style - Normal")]
        [SerializeField] private Color normalColor = Color.white;
        [Header("Style - Empty")]
        [SerializeField] private Material emptyMaterial = null;
        [SerializeField] private Color emptyColor = Color.black;

        int lastAmount = -1;

        public VisibleItemStack Stack { get; private set; }

        private List<DisplayedIcon> icons = new List<DisplayedIcon>();

        private void Awake()
        {
            icons.Add(new DisplayedIcon(icon));
        }

        public void Set(VisibleItemStack stack, bool showEmpty = true)
        {
            Stack = stack;
            count.text = stack.Amount.ToString();

            if (animation)
            {
                if (lastAmount < stack.Amount && lastAmount != -1)
                {
                    transform.DOKill(true);
                    Sequence seq = DOTween.Sequence();
                    seq.SetTarget(transform);
                    seq.Append(transform.DOScale(new Vector3(1.4f, 0.6f), 0.1f));
                    seq.Append(transform.DOScale(new Vector3(0.75f, 1.2f), 0.1f));
                    seq.Append(transform.DOScale(new Vector3(1.0f, 1.0f), 0.1f));
                }
                else if (lastAmount > stack.Amount && lastAmount != -1)
                {
                    transform.DOKill(true);
                    Sequence seq = DOTween.Sequence();
                    seq.SetTarget(transform);
                    seq.Append(transform.DOBlendableMoveBy(Vector3.down * 10, 0.2f));
                    seq.Append(transform.DOBlendableMoveBy(Vector3.down * -10, 0.1f));
                } 
            }
            lastAmount = stack.Amount;

            if (stack.Amount == 0)
            {
                icon.sprite = stack.Items[0].Sprite;
                icon.material = showEmpty ? emptyMaterial : icon.defaultMaterial;
                icon.color = showEmpty ? emptyColor : normalColor;
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
            RectTransform rt = icons[index].RectTransform;
            RectTransform rootCanvas = icons[index].Icon.canvas.transform as RectTransform;
            Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(rootCanvas, rt);

            position = b.center;
            size = b.size;
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
