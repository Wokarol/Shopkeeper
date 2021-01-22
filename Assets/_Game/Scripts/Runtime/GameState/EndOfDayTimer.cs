using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shopkeeper
{
    public class EndOfDayTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeLabel = null;
        [SerializeField] private float timePerDay = 120;
        private float timeLeft = 0;


        private void Start()
        {
            timeLeft = timePerDay;
        }

        private void Update()
        {
            if (timeLeft <= 0)
                return;

            if (WorldContext.Cheats.StopTime)
                return;

            timeLeft -= Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(timeLeft);
            timeLabel.text = $"{span.Minutes}:{span.Seconds:d2}";

            if(timeLeft <= 0)
                WorldContext.GameState.EndDay();
        }
    }
}
