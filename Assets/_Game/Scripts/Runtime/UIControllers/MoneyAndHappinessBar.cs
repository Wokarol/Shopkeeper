using DG.Tweening;
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
        [SerializeField] private RectTransform moneyIcon;
        [SerializeField] private TextMeshProUGUI happinessLabel = null;
        [SerializeField] private RectTransform happinessIcon;
        [Space]
        [SerializeField] private bool animateNumberShow = false;
        [SerializeField] private float numberIncreaseSpeed = 100;

        PlayerState playerState;

        static public RectTransform MoneyIcon;
        static public RectTransform HappinessIcon;

        private void Start()
        {
            MoneyIcon = moneyIcon;
            HappinessIcon = happinessIcon;
            playerState = WorldContext.PlayerState;

            playerState.MoneyChanged += m =>
            {
                UpdateMoney(m);
            };
            playerState.HappinessChanged += h =>
            {
                UpdateHappiness(h);
            };

            if (animateNumberShow)
            {
                DOVirtual.Float(0, playerState.Money, numberIncreaseSpeed, (f) =>
                {
                    UpdateMoney(Mathf.CeilToInt(f));
                }).SetSpeedBased();
                DOVirtual.Float(0, playerState.ClientHappiness, numberIncreaseSpeed, (f) =>
                {
                    UpdateHappiness(Mathf.CeilToInt(f));
                }).SetSpeedBased();
            }
            else
            {
                UpdateMoney(playerState.Money);
                UpdateHappiness(playerState.ClientHappiness);
            }
        }

        private void UpdateMoney(int value)
        {
            if (moneyLabel != null)
                moneyLabel.text = value.ToString();
        }

        private void UpdateHappiness(int value)
        {
            if (happinessLabel != null)
                happinessLabel.text = value.ToString();
        }
    }
}
