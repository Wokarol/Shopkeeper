using Shopkeeper.World;
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
        private CanvasGroup buttonCanvasGroup;
        private CraftingMaterials materials;

        public event Action<CraftingRecipe> RecipeSelected;

        public void Init(CraftingRecipe recipe)
        {
            this.recipe = recipe;
            ingredientList.Init(recipe);
            result.sprite = recipe.Result.Sprite;

            addButton.onClick.AddListener(OnAddButton);

            addButton.TryGetComponent(out buttonCanvasGroup);

            materials = WorldContext.PlayerState.CraftingMaterials;
            materials.Changed += (i, c) => UpdateButtonState();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            bool canCraft = materials.Contains(recipe.Ingredients);
            if (buttonCanvasGroup != null)
            {
                buttonCanvasGroup.alpha = canCraft ? 1 : 0.25f;
            }
            addButton.interactable = canCraft;
        }

        public void OnAddButton()
        {
            RecipeSelected?.Invoke(recipe);
        }
    }
}
