using CommandTerminalPlus;
using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shopkeeper
{
    public static class CraftingCommands
    {
        [RegisterCommand(Name = "show_crafting_materials", MaxArgCount = 1, Help = "Shows all craftable materials currently available, pass true to show empty elements")]
        public static void CurrentCraftingItemsCount(CommandArg[] args)
        {
            bool showZeroes = false;
            if (args.Length != 0)
            {
                showZeroes = args[0].Bool;
            }

            var allValues = WorldContext.PlayerState.CraftingMaterials.AllValues;
            int longestName = allValues.Select(v => v.Key.Name.Length).Max();

            StringBuilder output = new StringBuilder();

            int i = -1;
            int columns = 3;
            foreach (var item in allValues)
            {
                if (showZeroes || item.Value != 0)
                {
                    i++;
                    if (i % columns == 0 && i != 0)
                        output.Append("\n");
                    else if (i != 0)
                        output.Append("  ");

                    output.Append($"[{item.Key.Name.PadRight(longestName)}] = {item.Value}");
                }
            }

            CommandTerminalPlus.Terminal.Log(output.ToString());
        }

        [RegisterCommand(Name = "add_crafting_item", MinArgCount = 1, MaxArgCount = 2, Help = "Add craftable item to inventory")]
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
                if (itemName.ToLower() == "all")
                    AddAllItemsImplementation(itemCount);
                else
                    AddItemImplementation(itemName, itemCount);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void AddAllItemsImplementation(int itemCount)
        {
            Crafting.CraftingMaterials materials = WorldContext.PlayerState.CraftingMaterials;
            foreach (var item in materials.AllValues.Select(i => i.Key).ToList())
            {
                materials[item] += itemCount;
            }
        }

        public static void AddItemImplementation(string itemName, int count)
        {
            Item item = ItemDatabase.LoadFromResources().Get(itemName);

            WorldContext.PlayerState.CraftingMaterials[item] += count;
        }
    }
}
