using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Shopkeeper
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderCardUI orderCard = null;
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
        [SerializeField] float cardMovementInterval = 0.05f;
        [SerializeField] float cardMoveUpDuration = 0.3f;
        [Header("Orders")]
        [SerializeField] private int ordersAtTheStart;
        [SerializeField] private int maxOrderCount;
        [SerializeField] private Vector2 orderAddIntervalMinMax;
        [SerializeField] private Vector2 startDelayMinMax;
        [SerializeField] private List<Order> orderPool;

        private List<OrderCardUI> createdOrders = new List<OrderCardUI>();

        private IEnumerator Start()
        {
            for (int i = 0; i < ordersAtTheStart; i++)
            {
                yield return new WaitForSeconds(0.25f);
                SpawnOrder(GetRandomOrder());
            }

            yield return new WaitForSeconds(Random.Range(startDelayMinMax.x, startDelayMinMax.y));

            while (true)
            {
                yield return new WaitUntil(() => createdOrders.Count < maxOrderCount);
                yield return new WaitForSeconds(Random.Range(orderAddIntervalMinMax.x, orderAddIntervalMinMax.y));
                SpawnOrder(GetRandomOrder());
            }
        }

        private Order GetRandomOrder()
        {
            int index = Random.Range(0, orderPool.Count);
            return orderPool[index];
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

                seq.Insert((i - index) * cardMovementInterval, 
                    ort.DOAnchorPosY(ort.anchoredPosition.y + spacing, cardMoveUpDuration)
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
