using DG.Tweening;
using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] private ItemContainer warehouse;
        [Header("Crafting Slots")]
        [SerializeField] private CraftingSlot craftingSlotPrefab;
        [SerializeField] private int slotAmount = 4;
        [Space]
        [SerializeField] private VerticalLayoutGroup slotHolder;
        [Header("  Animation")]
        [SerializeField] private float showInterval = 0.2f;
        [SerializeField] private float showDurationPerElement = 0.5f;
        [SerializeField] private Ease showEase = Ease.OutCubic;
        [Header("  Selecting")]
        [SerializeField] private CraftingSelectorPanel selectorPanel;
        
        private readonly List<CraftingSlot> slots = new List<CraftingSlot>();
        private CraftingSlot overrideNextSlot;

        private CraftingMaterials materials;

        private void Start()
        {
            materials = WorldContext.PlayerState.CraftingMaterials;

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
            if(!materials.Contains(recipe.Ingredients))
            {
                Debug.Log("Not enough minerals");
                return;
            }


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

            if (slot == null)
                return;

            materials.Remove(recipe.Ingredients);
            slot.StartCrafting(recipe, warehouse.Add);
        }

        private void InitializeSlots(int slotAmount)
        {
            for (int i = 0; i < slotAmount; i++)
            {
                SpawnSlot(i);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(slotHolder.transform as RectTransform);
            slotHolder.enabled = false;

            for (int i = 0; i < slotAmount; i++)
            {
                AnimateSlot(slots[i], i);
            }
        }

        private void SpawnSlot(int index)
        {
            var slot = Instantiate(craftingSlotPrefab, slotHolder.transform);
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
