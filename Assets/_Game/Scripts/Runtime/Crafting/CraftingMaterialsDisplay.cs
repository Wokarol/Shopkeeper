using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class CraftingMaterialsDisplay : ItemLister
    {
        [Space]
        [SerializeField] ItemGroup itemsToShow = null;

        public CraftingMaterials Materials { get; set; }

        Dictionary<Item, ListedItem> listedItemsByItem = new Dictionary<Item, ListedItem>();

        private void Start()
        {
            if(Materials == null)
                Materials = WorldContext.PlayerState.CraftingMaterials;

            if(Materials.IsEmpty)
            {
                foreach (var item in itemsToShow.Items)
                {
                    Materials.Add(item);
                    Materials[item] = 10;
                }
            }

            IReadOnlyList<Item> items = itemsToShow.Items;
            Init(items.Count);

            for (int i = 0; i < itemsToShow.Items.Count; i++)
            {
                Item item = itemsToShow.Items[i];
                listedItemsByItem.Add(item, this[i]);
                this[i].Set(new VisibleItemStack(item, Materials[item]), false);
            }

            Materials.Changed += UpdateItemCount;
        }

        private void OnDestroy()
        {
            Materials.Changed -= UpdateItemCount;
        }

        private void UpdateItemCount(Item item, int amount)
        {
            if(listedItemsByItem.TryGetValue(item, out ListedItem listedItem))
            {
                VisibleItemStack stack = listedItem.Stack;
                stack.Amount = amount;
                listedItem.Set(stack);
            }
        }
    }
}
