using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private CraftingSlot craftingSlotPrefab;
        [SerializeField] private int slotAmount = 4;

        private List<CraftingSlot> slots = new List<CraftingSlot>();

        private IEnumerator Start()
        {
            for (int i = 0; i < slotAmount; i++)
            {
                SpawnSlot();
            }
            yield break;
        }

        private void SpawnSlot()
        {
            var slot = Instantiate(craftingSlotPrefab, transform);
            slots.Add(slot);
        }
    }
}
