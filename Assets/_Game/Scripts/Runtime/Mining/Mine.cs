using DG.Tweening;
using Shopkeeper.Crafting;
using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Mining
{

    public class Mine : MonoBehaviour
    {
        [System.Flags]
        enum ButtonType { None = 0, Mine = 1, Take = 2, Leave = 4, Any = Mine | Take | Leave }

        [SerializeField] private CraftingMaterialsDisplay display;
        [SerializeField] private FillAmountSlider fillSlider;
        [SerializeField] private ItemGroup craftingMaterialsToGet;
        [Space]
        [SerializeField] private RectTransform goldCount;
        [SerializeField] private Image goldCountIcon;
        [Space]
        [SerializeField] private Button mainButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private Button takeButton;
        [SerializeField] private TextMeshProUGUI mainButtonLabel;
        [Header("Messages")]
        [SerializeField, TextArea(1, 3)] private string startMineMessage;
        [SerializeField, TextArea(1, 3)] private string goDeeperMessage;
        [Header("Balancing")]
        [SerializeField] private int mineCost = 50;

        CraftingMaterials currentYield;
        private PlayerState playerState;
        private ButtonType clickInQueue;

        private void Awake()
        {
            currentYield = new CraftingMaterials();
            playerState = WorldContext.PlayerState;

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
            mainButton.onClick.AddListener(() => clickInQueue = ButtonType.Mine);
            leaveButton.onClick.AddListener(() => clickInQueue = ButtonType.Leave);
            takeButton.onClick.AddListener(() => clickInQueue = ButtonType.Take);

            StartCoroutine(Mine_Co());
        }

        private IEnumerator Mine_Co()
        {
            while (true)
            {
                mainButtonLabel.text = string.Format(startMineMessage, mineCost);
                yield return WaitForButton(ButtonType.Mine | ButtonType.Leave);

                switch (clickInQueue)
                {
                    case ButtonType.Mine:

                        if (playerState.Money > mineCost)
                        {
                            playerState.Money -= mineCost;
                            yield return DoMining();
                        }
                        else
                        {
                            goldCount.DOShakePosition(0.3f, 10, vibrato: 40);
                        }
                        break;
                    case ButtonType.Leave:
                        WorldContext.GameState.StartDay();
                        yield break;
                }

            }
        }

        public void CollectYield()
        {
            WorldContext.PlayerState.CraftingMaterials.Add(currentYield);
            currentYield.ZeroOut();
        }

        public IEnumerator DoMining()
        {
            while (true)
            {
                mainButtonLabel.text = goDeeperMessage;

                MineResources(5, 11);

                if (currentYield.CountAll() > 50)
                {
                    yield return new WaitForSeconds(0.1f);
                    currentYield.ZeroOut();
                    break;
                }

                yield return WaitForButton(ButtonType.Any);

                switch (clickInQueue)
                {
                    case ButtonType.Mine:                   
                        break;
                    case ButtonType.Leave:
                        CollectYield();
                        WorldContext.GameState.StartDay();
                        yield break;
                    case ButtonType.Take:
                        CollectYield();
                        yield break;
                }
            }
        }

        private void MineResources(int min, int max)
        {
            int resourcesGot = Random.Range(min, max);
            for (int i = 0; i < resourcesGot; i++)
            {
                int randomIndex = Random.Range(0, craftingMaterialsToGet.Items.Count);
                Item itemToAdd = craftingMaterialsToGet.Items[randomIndex];
                currentYield[itemToAdd] += 1;
            }
        }

        private IEnumerator WaitForButton(ButtonType mask)
        {
            clickInQueue = ButtonType.None;
            yield return new WaitUntil(() => mask.HasFlag(clickInQueue) && clickInQueue != ButtonType.None);
        }
    }
}
