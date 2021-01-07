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
        private Item item;

        public void Init(Item item)
        {
            this.item = item;
            image.sprite = item.Sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.material = dragMaterial;
            image.color = new Color(0, 0, 0, 0.5f);

            pickedItem = new GameObject("Picked Item").AddComponent<Image>();
            pickedItem.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

            RectTransform rt = (RectTransform)pickedItem.transform;
            rt.SetParent(image.canvas.transform);
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
            Vector3 animationTarget = Vector3.zero;

            if(TryGetTarget(eventData, rt, out IDragAndDropTarget target))
            {
                target.GetTargetPosition(item, out Vector3 position, out Vector2 size);
                DropOnto(rt, position, 0.2f, size, () =>
                    {
                        target.Dropped(item);
                        Destroy(gameObject);
                    });
            }
            else
            {
                DropOnto(rt, transform.position, 0.2f, Vector2.one * 100);
            }

        }

        private void DropOnto(RectTransform rt, Vector3 targetPosition, float duration, Vector2 sizeDelta, Action onFinished = null)
        {
            rt.DOSizeDelta(sizeDelta, duration);
            rt.DOScale(1f, duration);
            rt.DOMove(targetPosition, duration).SetEase(Ease.InOutCirc)
                .OnComplete(() =>
                {
                    image.material = image.defaultMaterial;
                    image.color = new Color(1, 1, 1, 1);
                    Destroy(pickedItem.gameObject);
                    onFinished?.Invoke();
                });
        }

        private bool TryGetTarget(PointerEventData eventData, RectTransform rt, out IDragAndDropTarget target)
        {
            if (pickedItem.canvas.TryGetComponent(out GraphicRaycaster caster))
            {
                List<RaycastResult> results = new List<RaycastResult>();
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = eventData.position
                };

                caster.Raycast(pointerData, results);
                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent(out target))
                    {
                        return true;
                    }
                }
            }

            target = null;
            return false;
        }
    }
}
