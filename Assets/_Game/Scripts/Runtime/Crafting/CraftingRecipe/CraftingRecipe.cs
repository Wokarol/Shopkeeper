using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    [CreateAssetMenu]
    public class CraftingRecipe : ScriptableObject
    {
        [SerializeField] private List<CraftingIngredient> ingredients;
        [SerializeField] private Item result;
    }

    [System.Serializable]
    public struct CraftingIngredient
    {
        public Item Item;
        public int Amount;
    }
}
