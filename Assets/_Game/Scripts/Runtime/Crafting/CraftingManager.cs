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

        private readonly List<CraftingSlot> slots = new List<CraftingSlot>();

        private IEnumerator Start()
        {
            VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();

            for (int i = 0; i < slotAmount; i++)
            {
                SpawnSlot();
            }

            Vector3 oldPos = layout.transform.position;
            layout.transform.position = Vector2.one * -4000;
            yield return null;
            layout.enabled = false;
            layout.transform.position = oldPos;

            for (int i = 0; i < slotAmount; i++)
            {
                AnimateSlot(slots[i], i);
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
