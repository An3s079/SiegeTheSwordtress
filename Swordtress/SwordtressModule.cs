using System;
using ItemAPI;

namespace Swordtress
{
    public class SwordtressModule : ETGModule
    {
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

                Log($"{SwordtressAssembly.ModName} v{SwordtressAssembly.Version} started successfully.", TEXT_COLOR);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }

        public static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
}
