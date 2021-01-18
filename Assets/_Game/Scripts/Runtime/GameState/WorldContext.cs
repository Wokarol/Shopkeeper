using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.World
{
    public static class WorldContext
    {
        public static PlayerState PlayerState { get; private set; }
        public static GameState GameState { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            PlayerState = new PlayerState();
            GameState = new GameState();
        }
    }
}