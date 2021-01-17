using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shopkeeper
{
    public class MoneyAndHappinessBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyLabel = null;
        [SerializeField] private TextMeshProUGUI happinessLabel = null;

        PlayerState playerState;

        private void Start()
        {
            playerState = WorldContext.PlayerState;

            playerState.MoneyChanged += m => UpdateMoney();
            playerState.HappinessChanged += m => UpdateHappiness();

            UpdateMoney();
            UpdateHappiness();
        }

        private void UpdateMoney()
        {
            moneyLabel.text = playerState.Money.ToString();
        }

        private void UpdateHappiness()
        {
            happinessLabel.text = playerState.ClientHappiness.ToString();
        }
    }
}
