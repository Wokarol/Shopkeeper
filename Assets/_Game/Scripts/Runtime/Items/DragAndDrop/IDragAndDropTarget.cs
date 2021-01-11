using System;
using UnityEngine;

namespace Shopkeeper
{
    internal interface IDragAndDropTarget
    {
        bool FindDropTarget(Item item, out Vector2 position, out Vector2 size, out Action onDropped);
    }
}