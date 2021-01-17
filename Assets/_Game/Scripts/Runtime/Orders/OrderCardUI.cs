using DG.Tweening;
using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    [SelectionBase]
    public class OrderCardUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI description = null;
        [SerializeField] private ItemLister itemLister = null;
        [Space]
        [SerializeField] private Button confirmButton = null;
        [SerializeField] private Button cancelButton = null;
        [Space]
        [SerializeField] private Image resultPanel = null;
        [SerializeField] private TextMeshProUGUI moneyLabel = null;
        [SerializeField] private TextMeshProUGUI happinessLabel = null;
        [Space]
        [SerializeField] private Color goodColor = Color.green;
        [SerializeField] private Color badColor = Color.red;

        private Order order;
        private PlayerState playerState;

        public event Action OnOrderFinished;

        private void Start()
        {
            playerState = WorldContext.PlayerState;

            resultPanel.gameObject.SetActive(false);
            confirmButton.onClick.AddListener(FinisheOrder);
            cancelButton.onClick.AddListener(() => ShowResult(badColor, 0, -25));
        }

        private void FinisheOrder()
        {
            if (order.IsFullfilled(itemLister))
            {
                ShowResult(goodColor, 100, 20);
            }
            else
            {
                ShowResult(badColor, 0, -50);
            }
        }

        private void ShowResult(Color color, int money, int happiness)
        {
            playerState.Money += money;
            playerState.ClientHappiness += happiness;

            resultPanel.gameObject.SetActive(true);
            resultPanel.color = color;
            moneyLabel.text = money.ToString();
            happinessLabel.text = happiness.ToString();

            Sequence seq = DOTween.Sequence();
            seq.Append(resultPanel.GetComponent<CanvasGroup>()
                .DOFade(0, 0.2f)
                .From()
                .SetEase(Ease.InOutCubic));
            seq.AppendInterval(0.5f);
            seq.AppendCallback(() =>
            {
                OnOrderFinished?.Invoke();
            });
        }

        public void Init(Order order)
        {
            description.text = order.Description;
            order.FillLister(itemLister);
            this.order = order;

            itemLister.FindDropTargetImplementation = FindDropTarget;
        }

        public bool FindDropTarget(Item item, out int i, out int innerItemIndex, out Action onDroppped)
        {
            return order.FindMatchingItem(itemLister, item, out i, out innerItemIndex, out onDroppped);
        }
    }
}
