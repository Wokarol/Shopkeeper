using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    [CreateAssetMenu(menuName = "Crafting Recipe Group")]
    public class CraftingRecipeGroup : ScriptableObject
    {
        [SerializeField] private List<CraftingRecipe> items = null;

        public IReadOnlyList<CraftingRecipe> Receipes => items;
    }
}
