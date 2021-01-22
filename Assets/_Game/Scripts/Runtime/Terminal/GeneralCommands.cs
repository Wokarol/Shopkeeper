using CommandTerminalPlus;
using Shopkeeper.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Terminal
{
    public static class GeneralCommands
    {
        [RegisterCommand(Name = "time_scale", MaxArgCount = 1, Help = "Gets or sets time scale")]
        public static void TimeScale(CommandArg[] args)
        {
            if(args.Length == 0)
            {
                CommandTerminalPlus.Terminal.Log($"Time Scale: {Time.timeScale}");
            }
            else
            {
                float scale = args[0].Float;
                Time.timeScale = scale;
            }
        }

        [RegisterCommand(Name = "end_day", MaxArgCount = 0, Help = "Ends the day")]
        public static void EndDay(CommandArg[] args)
        {
            WorldContext.GameState.EndDay();
        }

        [RegisterCommand(Name = "stop_time", MaxArgCount = 1, Help = "Tells if 'end of day' timer should stop")]
        public static void CountTime(CommandArg[] args)
        {
            if (args.Length == 0)
            {
                CommandTerminalPlus.Terminal.Log($"Stop Time: {WorldContext.Cheats.StopTime}");
            }
            else
            {
                WorldContext.Cheats.StopTime = args[0].Bool;
            }
        }
    }
}
