using UnityEngine;

namespace Shopkeeper
{
    [System.Serializable]
    public class ExactItem : ItemRequirement
    {
        [Header("Exact Item")]
        public Item Item;

        public override void AddItem(ListedItem listedItem, Item item)
        {
            var current = listedItem.Stack;
            current.Amount += 1;
            listedItem.Set(current);
        }

        public override int GetIndexOnItemListedItemForAnimation(ListedItem listedItem, Item item)
        {
            return 0;
        }

        public override void InitializeListedItem(ListedItem listedItem)
        {
            listedItem.Set(new ItemStack(Item, 0));
        }

        public override bool IsItemAccepted(ListedItem listedItem, Item item)
        {
            return item == Item;
        }
    }
}
