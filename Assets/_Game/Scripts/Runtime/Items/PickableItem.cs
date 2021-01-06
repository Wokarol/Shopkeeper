using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shopkeeper
{
    public class PickableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Material dragMaterial = null;

        private Image pickedItem;

        public void Init(Item item)
        {
            image.sprite = item.Sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.material = dragMaterial;
            image.color = new Color(0, 0, 0, 0.5f);

            pickedItem = new GameObject("Picked Item").AddComponent<Image>();
            pickedItem.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

            RectTransform rt = (RectTransform)pickedItem.transform;
            rt.parent = transform.root;
            rt.localScale = Vector3.one;
            rt.sizeDelta = Vector2.one * 100;
            rt.position = transform.position;
            pickedItem.sprite = image.sprite;

            rt.DOScale(1.4f, 0.25f).SetEase(Ease.OutBack);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform rt = (RectTransform)pickedItem.transform;
            rt.position += (Vector3)eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RectTransform rt = (RectTransform)pickedItem.transform;
            rt.DOScale(1f, 0.2f);
            rt.DOMove(transform.position, 0.2f).SetEase(Ease.InOutCirc)
                .OnComplete(() =>
                {
                    image.material = image.defaultMaterial;
                    image.color = new Color(1, 1, 1, 1);
                    Destroy(pickedItem.gameObject);
                });


        }
    }
}
