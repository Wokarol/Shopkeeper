using System;
using System.Collections.Generic;

namespace Shopkeeper.Crafting
{
    public class CraftingMaterials
    {
        private Dictionary<Item, int> materials = new Dictionary<Item, int>();

        public event Action<Item, int> Changed;

        public int this[Item item]
        {
            get
            {
                return materials.TryGetValue(item, out int value) 
                    ? value 
                    : 0;
            }
            set
            {
                if (materials.ContainsKey(item))
                {
                    materials[item] = value;
                    Changed(item, value);
                }
                else
                {
                    throw new System.Exception("Trying to set item that does not exist in crafting materials list");
                }
            }
        }

        public void Add(Item item)
        {
            materials.Add(item, 0);
            Changed?.Invoke(item, 0);
        }
    }
}