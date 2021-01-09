using UnityEngine;

namespace Shopkeeper
{
    internal interface IDragAndDropTarget
    {
        bool Accepts(Item item);
        void GetTargetPosition(Item item, out Vector3 position, out Vector2 size);
        void Dropped(Item item);
    }
}