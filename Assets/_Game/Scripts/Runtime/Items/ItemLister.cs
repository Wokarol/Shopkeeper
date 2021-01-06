using System;
using UnityEngine;

namespace Shopkeeper
{
    [SelectionBase]
    public class ItemLister : MonoBehaviour
    {
        [SerializeField] private ListedItem listedItemPrefab;

        internal void Add(ItemStack stack)
        {
            var listedItem = Instantiate(listedItemPrefab, transform);
            stack.Amount = 0;
            listedItem.Set(stack);
        }
    }
}