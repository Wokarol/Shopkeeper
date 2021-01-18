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

        CraftingMaterials materials;

        Dictionary<Item, ListedItem> listedItemsByItem = new Dictionary<Item, ListedItem>();

        private void Start()
        {
            materials = WorldContext.PlayerState.CraftingMaterials;

            if(materials.IsEmpty)
            {
                foreach (var item in itemsToShow.Items)
                {
                    materials.Add(item);
                    materials[item] = 10;
                }
            }

            IReadOnlyList<Item> items = itemsToShow.Items;
            Init(items.Count);

            for (int i = 0; i < itemsToShow.Items.Count; i++)
            {
                Item item = itemsToShow.Items[i];
                listedItemsByItem.Add(item, this[i]);
                this[i].Set(new VisibleItemStack(item, materials[item]), false);
            }

            materials.Changed += UpdateItemCount;
        }

        private void OnDestroy()
        {
            materials.Changed -= UpdateItemCount;
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
