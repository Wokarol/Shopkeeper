using DG.Tweening;
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

        public CraftingManager Manager;

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

        public void StartCrafting(CraftingRecipe recipe)
        {
            activePanel.SetActive(true);
            inactivePanel.SetActive(false);
            IsCrafting = true;

        }
    }
}
