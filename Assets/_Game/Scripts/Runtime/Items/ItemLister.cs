using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [SelectionBase]
    public class ItemLister : MonoBehaviour, IDragAndDropTarget
    {
        [SerializeField] private ListedItem listedItemPrefab;

        private List<ListedItem> createdItems = new List<ListedItem>();

        public Predicate<Item> AcceptsCheck;
        //public Action<Item> OnDropped;
        public Func<Item, int> IndexOfItemGetter;

        internal void Add(ItemStack stack)
        {
            ListedItem listedItem = Instantiate(listedItemPrefab, transform);
            stack.Amount = 0;
            listedItem.Set(stack);

            createdItems.Add(listedItem);
        }

        public bool Accepts(Item item) => AcceptsCheck?.Invoke(item) ?? true;

        public void Dropped(Item item)// => OnDropped?.Invoke(item);
        {
            ListedItem listedItem = createdItems[IndexOfItemGetter?.Invoke(item) ?? 0];

            ItemStack stack = listedItem.Stack;
            stack.Amount += 1;

            listedItem.Set(stack);
        }

        public void GetTargetPosition(Item item, out Vector3 position, out Vector2 size)
        {
            ListedItem listedItem = createdItems[IndexOfItemGetter?.Invoke(item) ?? 0];
            listedItem.GetIconPlacing(out position, out size);
        }
    }
}