using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Shopkeeper.World
{
    public class GameState
    {
        private SceneLookup sceneLookup;

        public GameState()
        {
            sceneLookup = SceneLookup.LoadFromResources();
        }

        internal void StartMine()
        {
            SceneManager.LoadScene(sceneLookup.Mine);
        }

        public void EndDay()
        {
            SceneManager.LoadScene(sceneLookup.EndOfDay);
        }

        public void StartDay()
        {
            SceneManager.LoadScene(sceneLookup.Main);
        }
    }
}
