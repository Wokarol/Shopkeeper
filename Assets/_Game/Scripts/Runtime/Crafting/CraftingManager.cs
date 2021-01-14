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

        private void Start()
        {
            InitializeSlots(slotAmount);
            selectorPanel.RecipeSelected += r =>
            {
                Debug.Log($"Recipe \"{r.name}\" selected");
            };
        }

        private void InitializeSlots(int slotAmount)
        {
            VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < slotAmount; i++)
            {
                SpawnSlot();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            layout.enabled = false;

            for (int i = 0; i < slotAmount; i++)
            {
                AnimateSlot(this.slots[i], i);
            }
        }

        private void SpawnSlot()
        {
            var slot = Instantiate(craftingSlotPrefab, transform);
            slots.Add(slot);
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
