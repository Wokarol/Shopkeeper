using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [SelectionBase]
    public class ItemLister : MonoBehaviour, IDragAndDropTarget
    {
        public delegate bool AcceptCheckWithTargetItem(Item item, out int listedItem, out int indexOfItemInsideListedItem, out Action onDropped);

        [SerializeField] private ListedItem listedItemPrefab;

        private readonly List<ListedItem> createdItems = new List<ListedItem>();

        public AcceptCheckWithTargetItem FindDropTargetImplementation;

        public ListedItem this[int i] => createdItems[i];

        public void Init(int numberOfItems)
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                ListedItem listedItem = Instantiate(listedItemPrefab, transform);
                createdItems.Add(listedItem);
            }
        }

        public bool FindDropTarget(Item item, out Vector2 position, out Vector2 size, out Action onDropped)
        {
            if (FindDropTargetImplementation(item, out int listedItem, out int indexOfItemInsideListedItem, out onDropped))
            {
                createdItems[listedItem].GetIconPlacing(indexOfItemInsideListedItem, out position, out size);
                return true;
            }
            else
            {
                position = Vector2.zero;
                size = Vector2.zero;
                return false;
            }
        }
    }
}