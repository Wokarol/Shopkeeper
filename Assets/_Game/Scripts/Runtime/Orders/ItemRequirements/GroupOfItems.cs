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
            current.Item = item;
            current.Amount += 1;
            listedItem.Set(current);
        }

        public override int GetIndexOnItemListedItemForAnimation(ListedItem listedItem, Item item)
        {
            return 0;
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
