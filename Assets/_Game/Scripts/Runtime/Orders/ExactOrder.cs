using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(menuName = "Orders/Exact Order")]
    public class ExactOrder : Order
    {
        [SerializeField, TextArea(3, 20)] private string description = "";
        [Space]
        [SerializeField] List<ItemStack> items = new List<ItemStack>();

        public override string Description => description;

        public override void FillLister(ItemLister itemLister)
        {
            foreach (var stack in items)
            {
                itemLister.Add(stack);
            }
        }
    }
}
