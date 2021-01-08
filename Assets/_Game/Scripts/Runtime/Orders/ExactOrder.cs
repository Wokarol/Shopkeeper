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

        public override int GetFirstFittingItemIndex(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].Item == item)
                {
                    return i;
                }
            }

            throw new System.Exception("No slot fits the item, that shouldn't happen");
        }

        public override bool IsItemAccepted(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
