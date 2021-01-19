using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class CraftingSelectorPanel : MonoBehaviour
    {
        [SerializeField] private CraftingRecipeGroup receipes;

        [Header("List")]
        [SerializeField] private RectTransform listTranform;
        [SerializeField] private CraftableItemUI craftableItemPrefab;
        [Space]
        [SerializeField] private float panelPeekMargin = 40;
        [Header("Space")]
        [SerializeField] private float openingDuration = 0.8f;
        [SerializeField] private Ease openingEase = Ease.OutBounce;
        [SerializeField] private float closingDuration = 0.8f;
        [SerializeField] private AnimationCurve closingEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private bool open;

        private RectTransform rectTransform;
        private Sequence openCloseSequence;

        public event Action<CraftingRecipe> RecipeSelected;
        public event Action<bool> Toggled;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            rectTransform.anchorMin = new Vector2(0, -1);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.anchoredPosition = new Vector2(0, panelPeekMargin);

            SpawnRecepies();
        }

        private void SpawnRecepies()
        {
            foreach (var recepie in receipes.Receipes)
            {
                var ui = Instantiate(craftableItemPrefab, listTranform);
                ui.Init(recepie);
                ui.RecipeSelected += r => RecipeSelected?.Invoke(r);
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Toggle();
            }
        }

        public void Show()
        {
            if (!open)
                Toggle();
        }

        public void Hide()
        {
            if (open)
                Toggle();
        }

        public void Toggle()
        {
            open = !open;

            Toggled?.Invoke(open);

            openCloseSequence.Kill();

            if (open)
            {
                float duration = openingDuration;
                openCloseSequence = DOTween.Sequence();

                openCloseSequence.Append(rectTransform.DOAnchorMax(Vector2.one, duration));
                openCloseSequence.Join(rectTransform.DOAnchorMin(Vector2.zero, duration));
                openCloseSequence.Join(rectTransform.DOAnchorPos(Vector2.zero, duration));

                openCloseSequence.SetEase(openingEase);
            }
            else
            {
                float duration = closingDuration;
                openCloseSequence = DOTween.Sequence();

                openCloseSequence.Append(rectTransform.DOAnchorMax(new Vector2(1, 0), duration));
                openCloseSequence.Join(rectTransform.DOAnchorMin(new Vector2(0, -1), duration));
                openCloseSequence.Join(rectTransform.DOAnchorPos(new Vector2(0, panelPeekMargin), duration));

                openCloseSequence.SetEase(closingEase);
            }
        }
    }
}
