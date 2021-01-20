using Shopkeeper.Crafting;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Mining
{

    public class Mine : MonoBehaviour
    {
        [SerializeField] private CraftingMaterialsDisplay display;
        [SerializeField] private FillAmountSlider fillSlider;
        [SerializeField] private ItemGroup craftingMaterialsToGet;
        [Space]
        [SerializeField] private Button mainButton;
        [SerializeField] private TextMeshProUGUI mainButtonLabel;
        [Header("Messages")]
        [SerializeField] private string startMineMessage;
        [SerializeField] private string goDeeperMessage;

        CraftingMaterials currentYield;
        private bool clickInQueue;

        private void Awake()
        {
            currentYield = new CraftingMaterials();
            if (currentYield.IsEmpty)
            {
                foreach (var item in craftingMaterialsToGet.Items)
                {
                    currentYield.Add(item);
                    currentYield[item] = 0;
                }
            }

            display.Materials = currentYield;
            fillSlider.MaxValue = 50;
            fillSlider.CurrentValue = 0;

            currentYield.Changed += (item, v) =>
            {
                fillSlider.CurrentValue = currentYield.CountAll();
            };
        }

        private void Start()
        {
            mainButton.onClick.AddListener(() => clickInQueue = true);

            StartCoroutine(Mine_Co());
        }

        private IEnumerator Mine_Co()
        {
            mainButtonLabel.text = startMineMessage;
            while (true)
            {
                yield return WaitForButton();
                mainButtonLabel.text = goDeeperMessage;

                int resourcesGot = Random.Range(5, 11);
                for (int i = 0; i < resourcesGot; i++)
                {
                    int randomIndex = Random.Range(0, craftingMaterialsToGet.Items.Count);
                    Item itemToAdd = craftingMaterialsToGet.Items[randomIndex];
                    currentYield[itemToAdd] += 1;
                }
            }
        }

        private IEnumerator WaitForButton()
        {
            clickInQueue = false;
            yield return new WaitUntil(() => clickInQueue);
            clickInQueue = false;
        }
    }
}
