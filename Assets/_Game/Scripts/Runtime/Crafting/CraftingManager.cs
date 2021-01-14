using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private CraftingSlot craftingSlotPrefab;
        [SerializeField] private int slotAmount = 4;
        [Header("Animation")]
        [SerializeField] private float showInterval = 0.2f;
        [SerializeField] private float showDurationPerElement = 0.5f;
        [SerializeField] private Ease showEase = Ease.OutCubic;
        [Header("Selecting")]
        [SerializeField] private CraftingSelectorPanel selectorPanel;
        private readonly List<CraftingSlot> slots = new List<CraftingSlot>();
        private CraftingSlot overrideNextSlot;

        private void Start()
        {
            InitializeSlots(slotAmount);
            selectorPanel.RecipeSelected += StartCraftingOf;
            selectorPanel.Toggled += open =>
            {
                if (!open)
                    overrideNextSlot = null;
            };
        }

        public void ShowSelectorFor(CraftingSlot craftingSlot)
        {
            overrideNextSlot = craftingSlot;
            selectorPanel.Show();
        }

        public void StartCraftingOf(CraftingRecipe recipe)
        {
            if (overrideNextSlot == null)
                Debug.Log($"Recipe \"{recipe.name}\" selected");
            else
                Debug.Log($"Recipe \"{recipe.name}\" will be started on \"{overrideNextSlot.name}\"");

            CraftingSlot slot = null;
            if(overrideNextSlot != null)
            {
                slot = overrideNextSlot;
                overrideNextSlot = null;
                selectorPanel.Hide();
            }
            else
            {
                foreach (var s in slots)
                {
                    if (!s.IsCrafting)
                    {
                        slot = s;
                        break;
                    }
                }
            }

            slot.StartCrafting(recipe);
        }

        private void InitializeSlots(int slotAmount)
        {
            VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < slotAmount; i++)
            {
                SpawnSlot(i);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            layout.enabled = false;

            for (int i = 0; i < slotAmount; i++)
            {
                AnimateSlot(slots[i], i);
            }
        }

        private void SpawnSlot(int index)
        {
            var slot = Instantiate(craftingSlotPrefab, transform);
            slot.name = $"Crafting Slot {index}";
            slots.Add(slot);
            slot.Manager = this;
        }

        private void AnimateSlot(CraftingSlot craftingSlot, int delay)
        {
            RectTransform rt = craftingSlot.transform as RectTransform;
            Vector3 pos = rt.position;
            pos.y = -10;
            rt.DOMove(pos, showDurationPerElement).From().SetDelay(delay * showInterval).SetEase(showEase);
        }
    }
}
