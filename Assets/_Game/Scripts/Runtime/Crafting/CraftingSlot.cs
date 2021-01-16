using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Crafting
{
    public class CraftingSlot : MonoBehaviour
    {
        [SerializeField] private GameObject activePanel;
        [SerializeField] private GameObject inactivePanel;
        [Space]
        [SerializeField] private Button startCrafting;
        [Header("Crafting UI")]
        [SerializeField] private Image finalResult = null;
        [SerializeField] private ItemLister input = null;
        [SerializeField] private RectTransform slider = null;

        public CraftingManager Manager { get; set; }

        public bool IsCrafting { get; set; } = false;

        private void Start()
        {
            activePanel.SetActive(false);
            inactivePanel.SetActive(true);

            startCrafting.onClick.AddListener(SelectRecipe);
        }

        private void SelectRecipe()
        {
            Manager.ShowSelectorFor(this);
        }

        public void StartCrafting(CraftingRecipe recipe, Action<Item> onComplete)
        {
            activePanel.SetActive(true);
            inactivePanel.SetActive(false);
            IsCrafting = true;

            finalResult.sprite = recipe.Result.Sprite;
            input.Init(recipe.Ingredients.Count);
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                CraftingIngredient ingredient = recipe.Ingredients[i];
                input[i].Set(new VisibleItemStack(ingredient.Item, ingredient.Amount));
            }

            DOVirtual.Float(0, 1, 3, SetSliderValue)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    StopCrafting();
                    onComplete(recipe.Result);
                });

            PopAnimation();
        }

        private void StopCrafting()
        {
            activePanel.SetActive(false);
            inactivePanel.SetActive(true);
            IsCrafting = false;
        }

        private void SetSliderValue(float t)
        {
            slider.anchorMax = Vector3.LerpUnclamped(new Vector3(0, 1), Vector3.one, t);
        }

        private void PopAnimation()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOBlendableLocalMoveBy(Vector3.right * 50, 0.3f).SetEase(Ease.OutCubic));
            seq.Append(transform.DOBlendableLocalMoveBy(Vector3.left * 50, 0.3f).SetEase(Ease.InOutCubic));
        }
    }
}
