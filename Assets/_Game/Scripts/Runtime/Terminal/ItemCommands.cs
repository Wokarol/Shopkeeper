using CommandTerminalPlus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shopkeeper.Terminal
{
    public static class ItemCommands
    {
        [RegisterCommand(Name = "list_items", MaxArgCount = 0, Help = "Shows all items in database")]
        public static void ListItems(CommandArg[] _)
        {
            IReadOnlyList<Item> items = ItemDatabase.LoadFromResources().Items;
            IEnumerable<string> names = items.Select(item => item.Name).OrderBy(x => x);
            Debug.Log(string.Join("  /  ", names));
        }

        public static void AddItemImplementation(string itemName, int count)
        {
            var container = Object.FindObjectOfType<ItemContainer>();
            Item item = ItemDatabase.LoadFromResources().Get(itemName);
            
            for (int i = 0; i < count; i++)
            {
                container.Add(item); 
            }
        }


        [RegisterCommand(Name = "add_item", MinArgCount = 1, MaxArgCount = 2, Help = "Adds item to item container")]
        public static void AddItem(CommandArg[] args)
        {
            string itemName = args[0].String;
            int itemCount = 1;

            if (args.Length >= 2)
            {
                itemCount = args[1].Int;
            }

            if (CommandTerminalPlus.Terminal.IssuedError) return;

            try
            {
                AddItemImplementation(itemName, itemCount);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        [RegisterCommand(Name = "clear_container", MaxArgCount = 0, Help = "Clears item container")]
        public static void ClearContainer(CommandArg[] args)
        {
            var container = Object.FindObjectOfType<ItemContainer>();
            container.Clear();
        }
    }
}
