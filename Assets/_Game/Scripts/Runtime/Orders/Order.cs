using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu]
    public class Order : ScriptableObject
    {
        [SerializeField, TextArea(3, 20)] private string description = "";
        [Space]
        [SerializeReference, SubclassSelector(typeof(ItemRequirement))] List<ItemRequirement> items = new List<ItemRequirement>();

        public string Description => description;
        public void FillLister(ItemLister itemLister)
        {
            itemLister.Init(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].InitializeListedItem(itemLister[i]);
            }
        }

        public bool FindMatchingItem(ItemLister itemLister, Item item, out int itemRequirementIndex, out int indexOfItemInRequirement, out Action onAdded)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsItemAccepted(itemLister[i], item))
                {
                    itemRequirementIndex = i;
                    indexOfItemInRequirement = items[i].GetIndexOnItemListedItemForAnimation(itemLister[i], item);
                    onAdded = () => items[i].AddItem(itemLister[i], item);
                    return true;
                }
            }
            itemRequirementIndex = 0;
            indexOfItemInRequirement = 0;
            onAdded = null;
            return false;
        }

        public bool IsFullfilled(ItemLister itemLister)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(!items[i].IsFullfilled(itemLister[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
