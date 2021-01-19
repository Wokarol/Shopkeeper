using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class EndOfDayBrain : MonoBehaviour
    {
        public void NextDay()
        {
            WorldContext.GameState.StartDay();
        }
    }
}
