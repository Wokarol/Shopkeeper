using DG.Tweening;
using Shopkeeper.World;
using System;
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

        [SerializeField] private List<Item> items; // To be yeeted out of inspector
        public int columnCount = 6;



        private void Start()
        {
            items = WorldContext.PlayerState.Items;

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                int x = i % columnCount;
                int y = i / columnCount;

                int dist = x + y;
                SpawnItemElement(item, dist * 0.1f);
            }
        }

        private PickableItem SpawnItemElement(Item item, float delay)
        {
            PickableItem image = Instantiate(itemPrefab, transform);
            image.Init(item);

            image.transform.DOScale(Vector3.zero, 0.5f).From().SetDelay(delay).SetEase(Ease.OutBack);

            return image;
        }

        public void Add(Item item)
        {
            items.Add(item);
            PickableItem i = SpawnItemElement(item, 0);
            i.OnDestroyed += i => items.Remove(i);
        }

        public void Clear()
        {
            items.Clear();
            foreach (Transform child in transform)
            {
                child.DOScale(0, 0.3f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(child.gameObject));
            }
        }
    }
}
