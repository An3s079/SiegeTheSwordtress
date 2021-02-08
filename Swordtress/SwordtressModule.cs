using GungeonAPI;
using ItemAPI;
using System;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace Swordtress
{
    public class SwordtressModule : ETGModule
    {
        public static bool SwordtressModeActive = false;

        
        public static List<PickupObject.ItemQuality> basePickUpQualityList = new List<PickupObject.ItemQuality>();
        public static int[] pickUpExceptions = new int[] { 73, 85, 78, 67, 600 };//add item id's for ones we are keeping here.
        public static Dictionary<int, PickupObject.ItemQuality> stsQualityItemDictionary = new Dictionary<int, PickupObject.ItemQuality>();
        public static List<int> stsItemIds = new List<int>();

        public static readonly string MOD_NAME = "Siege The Swordtress";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {
            try
            {
                ShrineFactory.Init();
                ItemBuilder.Init();
                
                /*
                items
                */
                stsItemIds.Add(ThiefCloak.Init());
                stsItemIds.Add(ThiefBomb.Init());
                stsItemIds.Add(SecondWind.Init());
                stsItemIds.Add(Bladelust.Init());
                stsItemIds.Add(SharpenedGrindstone.Init());
                stsItemIds.Add(CatsCowl.Init());
                stsItemIds.Add(CritItem.Init());
                stsItemIds.Add(BookOfParrying.Init());
                stsItemIds.Add(BerserkersWarCry.Init());
                stsItemIds.Add(RoseCharm.Init()); //this item will maybe be really bad but we will see.

                /*
                weapons
                */
                stsItemIds.Add(Katana.Add());
                stsItemIds.Add(RoyalGuardsKnife.Add());
                stsItemIds.Add(ThrowingKnives.Add());

                for (int i = 0; i < stsItemIds.Count; i++)
                {
                    stsQualityItemDictionary.Add(stsItemIds[i], PickupObjectDatabase.GetById(stsItemIds[i]).quality);
                    PickupObjectDatabase.GetById(stsItemIds[i]).quality = PickupObject.ItemQuality.EXCLUDED;
                }
                for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
                {
                    if (PickupObjectDatabase.Instance.Objects[i] != null)
                    {
                        basePickUpQualityList.Add(PickupObjectDatabase.Instance.Objects[i].quality);
                    }
                    else
                    {
                        basePickUpQualityList.Add(PickupObject.ItemQuality.EXCLUDED);//so lists will match up
                    }
                }

                SwordtressBreachShrine.Add();
                ShrineFactory.PlaceBreachShrines();
                Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.Message);
                ETGModConsole.Log(e.StackTrace);
            }

        }
        public static void activateStS()
        {
            for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
            {
                if (PickupObjectDatabase.Instance.Objects[i] != null)
                {
                    PickupObjectDatabase.Instance.Objects[i].quality = PickupObject.ItemQuality.EXCLUDED;
                }
            }
            foreach (int id in stsQualityItemDictionary.Keys)
            {
                PickupObjectDatabase.GetById(id).quality = stsQualityItemDictionary[id];
            }
            for (int i = 0; i < pickUpExceptions.Length; i++)
            {
                PickupObjectDatabase.GetById(pickUpExceptions[i]).quality = basePickUpQualityList[pickUpExceptions[i]];
            }
        }
        public static void disableStS()
        {
            for (int i = 0; i < basePickUpQualityList.Count; i++)
            {
                if (PickupObjectDatabase.GetById(i) != null)
                {
                    PickupObjectDatabase.GetById(i).quality = basePickUpQualityList[i];
                }
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
