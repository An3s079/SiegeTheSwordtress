using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GungeonAPI;
using UnityEngine;
using ItemAPI;
namespace Swordtress
{
    class SwordtressBreachShrine
    {
        public static void Add()
        {
            ShrineFactory sf = new ShrineFactory()
            {

                name = "Swordtress Shrine",
                modID = "sts",
                text = "A shrine from a distant planet.",
                spritePath = "Swordtress/Resources/StsModeShrine.png",
                acceptText = "Togge Swordtress mode.",
                declineText = "Return.",
                OnAccept = Accept,
                OnDecline = null,
                //offset = new Vector3(43.8f, 42.4f, 42.9f),
                offset = new Vector3((int)43, 17.4f, 0),
                talkPointOffset = new Vector3(0, 0, 0),
                isToggle = false,
                isBreachShrine = true
            };
            //register shrine
            sf.Build();
        }


        private static bool CanUse(PlayerController player, GameObject npc)
        {
            return true;
        }

        public static void Accept(PlayerController player, GameObject npc)
        {
            
            if (SwordtressModule.SwordtressModeActive == true)
            {
                string header = "Swordtress Mode Disabled";
                string text = "";
                Notify(header, text);
                SwordtressModule.disableStS();
               SwordtressModule.SwordtressModeActive = false;
            }
            else
            {
                string header = "Swordtress Mode Enabled";
                string text = "";
                Notify(header, text);
                SwordtressModule.activateStS();
                                SwordtressModule.SwordtressModeActive = true;
            }
        }
        private static void Notify(string header, string text)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName("Swordtres/Resources/doesntexistyet.png");
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.PURPLE, false, true);
        }
    }
}

