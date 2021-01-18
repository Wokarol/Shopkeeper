using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu]
    public class SceneLookup : ScriptableObject
    {
        public string Menu = "Menu";
        public string Main = "Main";
        public string EndOfDay = "End Of Day";

        public static SceneLookup LoadFromResources()
        {
            SceneLookup[] lookupsFound = Resources.LoadAll<SceneLookup>("");
            if (lookupsFound.Length == 0)
            {
                throw new System.Exception("There is no Item Database to load");
            }

            SceneLookup database = lookupsFound[0];
            return database;
        }
    }
}
