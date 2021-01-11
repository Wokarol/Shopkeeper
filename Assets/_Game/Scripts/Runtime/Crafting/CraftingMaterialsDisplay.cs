using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class CraftingMaterialsDisplay : ItemLister
    {
        [Space]
        [SerializeField] ItemGroup craftingItems = null;

        private void Start()
        {
            IReadOnlyList<Item> items = craftingItems.Items;
            Init(items.Count);
            for (int i = 0; i < craftingItems.Items.Count; i++)
            {
                this[i].Set(new VisibleItemStack(craftingItems.Items[i], 0), false);
            }
        }
    }
}
