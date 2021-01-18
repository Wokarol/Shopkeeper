using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shopkeeper
{
    public class MenuBrain : MonoBehaviour
    {
        public void StartGame()
        {
            WorldContext.GameState.StartDay();
        }
    }
}
