using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shopkeeper
{
    [System.Serializable]
    public class GroupOfItems : ItemRequirement
    {
        [Header("Group of Items")]
        public ItemGroup Group;

        public override void AddItem(ListedItem listedItem, Item item)
        {
            var current = listedItem.Stack;

            if(current.Amount == 0)
            {
                current.Items = new List<Item> { item };
            }
            else
            {
                if (!current.Items.Contains(item))
                    current.Items.Add(item);
            }

            
            current.Amount += 1;
            listedItem.Set(current);
        }

        public override int GetIndexOnItemListedItemForAnimation(ListedItem listedItem, Item item)
        {
            List<Item> items = listedItem.Stack.Items;
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i] == item)
                {
                    return i;
                }
            }
            return items.Count - 1;
        }

        public override void InitializeListedItem(ListedItem listedItem)
        {
            listedItem.Set(new VisibleItemStack(Group.Items[0], 0));
        }

        public override bool IsItemAccepted(ListedItem listedItem, Item item)
        {
            return Group.Items.Contains(item);
        }
    }
}
