namespace Shopkeeper
{
    [System.Serializable]
    public abstract class ItemRequirement
    {
        public abstract bool IsItemAccepted(ListedItem listedItem, Item item);
        public abstract void InitializeListedItem(ListedItem listedItem);
        public abstract void AddItem(ListedItem listedItem, Item item);
        public abstract int GetIndexOnItemListedItemForAnimation(ListedItem listedItem, Item item);
    }
}
