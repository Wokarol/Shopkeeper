using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Crafting
{
    public class CraftingSlot : MonoBehaviour
    {
        [SerializeField] private GameObject activePanel;
        [SerializeField] private GameObject inactivePanel;

        private void Start()
        {
            activePanel.SetActive(false);
            inactivePanel.SetActive(true);
        }
    }
}
