using Shopkeeper.Crafting;
using System;
using System.Collections.Generic;

namespace Shopkeeper.World
{
    public class PlayerState
    {
        public CraftingMaterials CraftingMaterials = new CraftingMaterials();
        public List<Item> Items = new List<Item>();

        private int clientHappiness = 100;
        private int money = 0;

        public int Money
        {
            get => money;
            set
            {
                if (money == value)
                    return;
                money = value;
                MoneyChanged?.Invoke(money);
            }
        }

        public int ClientHappiness
        {
            get => clientHappiness;
            set
            {
                if (clientHappiness == value)
                    return;
                if (value > 0)
                    clientHappiness = value;
                else
                    clientHappiness = 0;
                HappinessChanged?.Invoke(clientHappiness);
            }
        }

        public event Action<int> MoneyChanged;
        public event Action<int> HappinessChanged;
    }
}