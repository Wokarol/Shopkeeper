using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class IngredientListUI : MonoBehaviour
    {
        [SerializeField] private IngredientUI ingredientPrefab;

        public void Init(CraftingRecipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                var ui = Instantiate(ingredientPrefab, transform);
                ui.Init(ingredient);
            }
        }
    }
}
