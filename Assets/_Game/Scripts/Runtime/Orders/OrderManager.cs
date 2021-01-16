using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderCardUI orderCard = null;
        [SerializeField] private List<Order> orders;            // Probably will be removed, used for testing for now
        [Space]
        [SerializeField] float paddingRight = 5;
        [SerializeField] float paddingTop = 5;
        [SerializeField] float spacing = 260;
        [Space]
        [SerializeField] float flyInDistance = 500;
        [SerializeField] float flyInDuration = 0.5f;
        [SerializeField] Ease flyInEase = Ease.OutCubic;
        [Space]
        [SerializeField] float flyOutDuration = 0.5f;
        [SerializeField] Ease flyOutEase = Ease.InBack;

        private List<OrderCardUI> createdOrders = new List<OrderCardUI>();

        private IEnumerator Start()
        {
            foreach (var order in orders)
            {
                yield return new WaitForSeconds(0.25f);
                SpawnOrder(order);
            }
        }

        private void SpawnOrder(Order order)
        {
            var card = Instantiate(orderCard, transform);
            card.Init(order);
            RectTransform rtransform = card.transform as RectTransform;
            Vector2 finalPos = new Vector2(-paddingRight, -(paddingTop + spacing * createdOrders.Count));

            rtransform.anchoredPosition = finalPos;
            rtransform.DOAnchorPosX(finalPos.x + flyInDistance, flyInDuration)
                .From()
                .SetEase(flyInEase);

            createdOrders.Add(card);

            card.OnOrderFinished += () => RemoveOrder(createdOrders.IndexOf(card));
        }

        private void RemoveOrder(int index)
        {
            RectTransform rt = createdOrders[index].transform as RectTransform;

            Sequence seq = DOTween.Sequence();

            seq.Append(rt.DOAnchorPosX(rt.anchoredPosition.x + flyInDistance, flyOutDuration)
                .SetEase(flyOutEase));

            for (int i = index + 1; i < createdOrders.Count; i++)
            {
                RectTransform ort = createdOrders[i].transform as RectTransform;

                seq.Insert(flyOutDuration + (i - index) * 0.05f, 
                    ort.DOAnchorPosY(ort.anchoredPosition.y + spacing, 0.3f)
                        .SetEase(Ease.InBack, 1.05f));
            }

            seq.OnComplete(() =>
            {
                Destroy(rt.gameObject);
                createdOrders.RemoveAt(index);
            });
        }
    }
}
