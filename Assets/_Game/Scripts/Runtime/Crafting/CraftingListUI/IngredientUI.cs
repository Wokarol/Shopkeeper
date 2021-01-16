using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper.Crafting
{
    public class IngredientUI : MonoBehaviour
    {
        [SerializeField] private Image image = null;
        [SerializeField] private TextMeshProUGUI count = null;

        public void Init(CraftingIngredient ingredient)
        {
            image.sprite = ingredient.Item.Sprite;
            count.text = ingredient.Amount.ToString();
        }
    }
}
