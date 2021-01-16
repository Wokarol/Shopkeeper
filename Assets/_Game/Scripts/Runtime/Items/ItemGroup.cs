using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    [CreateAssetMenu(menuName = "Item Group")]
    public class ItemGroup : ScriptableObject
    {
        [SerializeField] private List<Item> items = null;

        public IReadOnlyList<Item> Items => items;
    }
}