using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Swordtress
{
    public class SwordtressModule : ETGModule
    {
        public static readonly string MOD_NAME = "Siege The Swordtress";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {
            try
            {
                ItemBuilder.Init();
                ThiefCloak.Init();
                ThrowingKnives.Add();
                ThiefBomb.Init();
                SecondWind.Init();
                Bladelust.Init();
                SharpenedGrindstone.Init();
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
