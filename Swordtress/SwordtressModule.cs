//using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
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
                //ShrineFactory.Init();
                ItemBuilder.Init();
                //items
                ThiefCloak.Init();
                ThrowingKnives.Add();
                ThiefBomb.Init();
                SecondWind.Init();
                Bladelust.Init();
                SharpenedGrindstone.Init();
                CatsCowl.Init();
                CritItem.Init();
                BookOfParrying.Init();
                BerserkersWarCry.Init();
                RoseCharm.Init(); //this item will maybe be really bad but we will see.

                //weapons
                Katana.Add();
                RoyalGuardsKnife.Add();


                //SwordtressBreachShrine.Add();
                //ShrineFactory.PlaceBreachShrines();
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
