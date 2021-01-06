using UnityEngine;

namespace Shopkeeper
{
    [System.Serializable]
    public struct ItemStack
    {
        public Item Item;
        public int Amount;
    }

    [CreateAssetMenu(menuName = "Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private Sprite sprite = null;
        [SerializeField] private string nameOverride = "";

        public Sprite Sprite => sprite;
        public string Name => string.IsNullOrEmpty(nameOverride) ? name : nameOverride;
    }
}