using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Crafting
{
    public class CraftableItemUI : MonoBehaviour
    {
        [SerializeField] private IngredientListUI ingredientList = null;
        [SerializeField] private Image result = null;
        [SerializeField] private Button addButton = null;
        private CraftingRecipe recipe;

        public event Action<CraftingRecipe> RecipeSelected;

        public void Init(CraftingRecipe recipe)
        {
            this.recipe = recipe;
            ingredientList.Init(recipe);
            result.sprite = recipe.Result.Sprite;

            addButton.onClick.AddListener(OnAddButton);
        }

        public void OnAddButton()
        {
            RecipeSelected?.Invoke(recipe);
        }
    }
}
