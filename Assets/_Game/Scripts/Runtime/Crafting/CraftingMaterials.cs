using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopkeeper.Crafting
{
    public class CraftingMaterials
    {
        private Dictionary<Item, int> materials = new Dictionary<Item, int>();

        public event Action<Item, int> Changed;
        public IEnumerable<KeyValuePair<Item, int>> AllValues => materials;
        public bool IsEmpty => materials.Count == 0;

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
                    Changed?.Invoke(item, value);
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

        public int CountAll()
        {
            return materials.Values.Sum();
        }

        internal bool Contains(IReadOnlyList<CraftingIngredient> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if(this[ingredient.Item] < ingredient.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        internal void Remove(IReadOnlyList<CraftingIngredient> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                this[ingredient.Item] -= ingredient.Amount;
            }
        }
    }
}