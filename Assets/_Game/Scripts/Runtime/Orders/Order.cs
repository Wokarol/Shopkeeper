using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shopkeeper
{
    public abstract class Order : ScriptableObject
    {
        public abstract string Description { get; }
        public abstract void FillLister(ItemLister itemLister);
    }
}
