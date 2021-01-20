using Shopkeeper.Crafting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Mining
{
    public class Mine : MonoBehaviour
    {
        [SerializeField] private CraftingMaterialsDisplay display;
        [SerializeField] private ItemGroup craftingMaterialsToGet;

        CraftingMaterials currentYield;

        private void Awake()
        {
            currentYield = new CraftingMaterials();
            if (currentYield.IsEmpty)
            {
                foreach (var item in craftingMaterialsToGet.Items)
                {
                    currentYield.Add(item);
                    currentYield[item] = 0;
                }
            }

            display.Materials = currentYield;
        }
    } 
}
