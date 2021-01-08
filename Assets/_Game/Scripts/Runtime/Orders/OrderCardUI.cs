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

        public void Init(Order order)
        {
            description.text = order.Description;
            order.FillLister(itemLister);
            itemLister.AcceptsCheck = order.IsItemAccepted;
            itemLister.IndexOfItemGetter = order.GetFirstFittingItemIndex;
        }
    }
}
