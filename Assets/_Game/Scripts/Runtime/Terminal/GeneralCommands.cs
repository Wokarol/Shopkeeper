using CommandTerminalPlus;
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
    }
}
