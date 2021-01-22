using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class MainMicsBrain : MonoBehaviour
    {
        public void EndDay()
        {
            WorldContext.GameState.EndDay();
        }
    }
}
