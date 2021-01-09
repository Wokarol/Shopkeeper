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
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private ItemLister itemLister;
        private Order order;

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
