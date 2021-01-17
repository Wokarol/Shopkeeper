using CommandTerminalPlus;
using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Terminal
{
    public static class MoneyAndHappinessCommands
    {
        [RegisterCommand(command_name: "add_money", Help = "Adds money to player's account", MaxArgCount = 1, MinArgCount = 1)]
        public static void AddMoney(CommandArg[] args)
        {
            int moneyToAdd = args[0].Int;

            if (CommandTerminalPlus.Terminal.IssuedError) return;

            WorldContext.PlayerState.Money += moneyToAdd;
        }

        [RegisterCommand(command_name: "add_happiness", Help = "Adds client's happiness to player's account", MaxArgCount = 1, MinArgCount = 1)]
        public static void AddHappiness(CommandArg[] args)
        {
            int happinessToAdd = args[0].Int;

            if (CommandTerminalPlus.Terminal.IssuedError) return;

            WorldContext.PlayerState.ClientHappiness += happinessToAdd;
        }
    }
}
