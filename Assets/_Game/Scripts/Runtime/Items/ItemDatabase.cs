using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Shopkeeper
{
    [CreateAssetMenu]
    public class ItemDatabase : ScriptableObject
    {
        public List<Item> items;

        private Dictionary<string, Item> nameToItemMap = new Dictionary<string, Item>();
        private bool isDirty;

        private void OnValidate()
        {
            isDirty = true;
        }

        public Item Get(string name)
        {
            if(isDirty)
            {
                nameToItemMap = items.ToDictionary(i => i.Name);
            }

            if (nameToItemMap.TryGetValue(name, out Item item))
            {
                return item;
            }
            else
            {
                throw new System.Exception("Cannot find specified item in database");
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Clear and add...")]
        private void ClearAndAddAllByLabel()
        {
            items.Clear();
            AddAllByLabel();
        }

        [ContextMenu("Add all items with label \"Item\"")]
        private void AddAllByLabel()
        {
            var itemsToLoad = AssetDatabase.FindAssets("l:Item")
                .Select(p => AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(p)));
            foreach (var item in itemsToLoad)
            {
                if(!items.Contains(item))
                {
                    items.Add(item);
                }
            }
        } 
#endif

        public static ItemDatabase LoadFromResources()
        {
            ItemDatabase database = Resources.FindObjectsOfTypeAll<ItemDatabase>()[0];
            return database;
        }
    }
}
