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
        [SerializeField] private List<Order> orders;
        [Space]
        [SerializeField] float paddingRight = 5;
        [SerializeField] float paddingTop = 5;
        [SerializeField] float spacing = 260;
        [Space]
        [SerializeField] float flyInDistance = 500;
        [SerializeField] Ease flyInEase = Ease.OutCubic;

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
            rtransform.DOAnchorPosX(finalPos.x + flyInDistance, 0.5f)
                .From()
                .SetEase(flyInEase);

            createdOrders.Add(card);
        }
    }
}
