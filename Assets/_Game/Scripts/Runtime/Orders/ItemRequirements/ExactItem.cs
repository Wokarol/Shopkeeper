using UnityEngine;

namespace Shopkeeper
{
    [System.Serializable]
    public class ExactItem : ItemRequirement
    {
        public Item Item;
        public int Amount;

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
            listedItem.Set(new VisibleItemStack(Item, 0));
        }

        public override bool IsFullfilled(ListedItem listedItem)
        {
            VisibleItemStack stack = listedItem.Stack;
            return stack.Amount >= 1;
        }

        public override bool IsItemAccepted(ListedItem listedItem, Item item)
        {
            return item == Item;
        }
    }
}
