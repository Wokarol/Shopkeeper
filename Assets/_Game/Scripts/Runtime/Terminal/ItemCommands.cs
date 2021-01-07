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

        public static void AddItemImplementation(string itemName)
        {
            var container = Object.FindObjectOfType<ItemContainer>();
            Item item = ItemDatabase.LoadFromResources().Get(itemName);

            container.Add(item);
        }


        [RegisterCommand(Name = "add_item", MinArgCount = 1, MaxArgCount = 1, Help = "Adds item to item container")]
        public static void AddItem(CommandArg[] args)
        {
            string itemName = args[0].String;

            if (CommandTerminalPlus.Terminal.IssuedError) return;

            try
            {
                AddItemImplementation(itemName);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
