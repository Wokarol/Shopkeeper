using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    [SelectionBase]
    public class OrderCardUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI description;
        [SerializeField] private ItemLister itemLister;
        [Space]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [Space]
        [SerializeField] private CanvasGroup correctPanel;
        [SerializeField] private CanvasGroup wrongPanel;

        private Order order;
        public event Action OnOrderFinished;

        private void Start()
        {
            correctPanel.gameObject.SetActive(false);
            wrongPanel.gameObject.SetActive(false);

            confirmButton.onClick.AddListener(FinisheOrder);
            cancelButton.onClick.AddListener(() => Debug.Log("Canceled"));
        }

        private void FinisheOrder()
        {
            if(order.IsFullfilled(itemLister))
            {
                ShowResult(correctPanel);
            }
            else
            {
                ShowResult(wrongPanel);
            }
        }

        private void ShowResult(CanvasGroup correctPanel)
        {
            correctPanel.gameObject.SetActive(true);

            correctPanel
                .DOFade(0, 0.2f)
                .From()
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => OnOrderFinished?.Invoke());
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
