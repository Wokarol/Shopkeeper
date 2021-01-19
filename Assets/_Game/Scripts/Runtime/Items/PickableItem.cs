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

        public event Action<Item> OnDestroyed;

        public void Init(Item item)
        {
            this.item = item;
            image.sprite = item.Sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (pickedItem != null)
            {
                eventData.pointerDrag = null;
                return;
            }

            image.material = dragMaterial;
            image.color = new Color(0, 0, 0, 0.5f);

            pickedItem = new GameObject("Picked Item").AddComponent<Image>();
            pickedItem.gameObject.AddComponent<CanvasGroup>().blocksRaycasts = false;

            RectTransform rt = (RectTransform)pickedItem.transform;
            rt.SetParent(image.canvas.transform);

            Bounds b = GetImageBounds();

            rt.localScale = Vector3.one;
            rt.sizeDelta = b.size;
            rt.position = transform.position;
            pickedItem.sprite = image.sprite;

            rt.DOScale(1.4f, 0.25f).SetEase(Ease.OutBack).SetId("Scale Tween");
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

            if (TryGetAcceptableTarget(eventData, out DropTarget dropTarget))
            {
                DropOnto(rt, dropTarget.Position, 0.2f, dropTarget.Size, () =>
                    {
                        dropTarget.OnDropped?.Invoke();
                        Destroy(gameObject);
                    });
            }
            else
            {
                Bounds b = GetImageBounds();

                DropOnto(rt, b.center, 0.2f, b.size);
            }
        }

        private Bounds GetImageBounds()
        {
            RectTransform myTransform = image.transform as RectTransform;
            RectTransform rootCanvas = image.canvas.transform as RectTransform;
            Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(rootCanvas, myTransform);
            return b;
        }

        private void DropOnto(RectTransform rt, Vector3 targetPosition, float duration, Vector2 sizeDelta, Action onFinished = null)
        {
            rt.DOKill();

            rt.DOSizeDelta(sizeDelta, duration);
            rt.DOScale(1f, duration);
            rt.DOLocalMove(targetPosition, duration).SetEase(Ease.InOutCirc)
                .OnComplete(() =>
                {
                    image.material = image.defaultMaterial;
                    image.color = new Color(1, 1, 1, 1);
                    Destroy(pickedItem.gameObject);
                    onFinished?.Invoke();
                    OnDestroyed?.Invoke(item);
                })
                .SetId("Returning Tween");
        }

        private bool TryGetAcceptableTarget(PointerEventData eventData, out DropTarget dropTarget)
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
                    if (result.gameObject.TryGetComponent(out IDragAndDropTarget target))
                    {
                        // Item is over IDragAndDropTarget
                        if (target.FindDropTarget(item, out Vector2 pos, out Vector2 size, out Action onDropped))
                        {
                            dropTarget = new DropTarget(target, pos, size, onDropped);
                            return true;
                        }
                    }
                }
            }

            dropTarget = new DropTarget();
            return false;
        }

        struct DropTarget
        {
            public IDragAndDropTarget Target;
            public Vector2 Position;
            public Vector2 Size;
            public Action OnDropped;

            public DropTarget(IDragAndDropTarget target, Vector2 position, Vector2 size, Action onDropped)
            {
                Target = target;
                Position = position;
                Size = size;
                OnDropped = onDropped;
            }
        }
    }
}
