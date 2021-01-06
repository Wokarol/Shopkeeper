using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    public class ItemContainer : MonoBehaviour
    {
        [SerializeField] private PickableItem itemPrefab;

        [SerializeField] private List<Item> items = new List<Item>(); // To be yeeted out of inspector
        public int columnCount = 6;

        private void Start()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                int x = i % columnCount;
                int y = i / columnCount;

                int dist = x + y;

                PickableItem image = Instantiate(itemPrefab, transform);
                image.Init(item);

                image.transform.DOScale(Vector3.zero, 0.5f).From().SetDelay(dist * 0.1f).SetEase(Ease.OutBack);
            }
        }
    }
}
