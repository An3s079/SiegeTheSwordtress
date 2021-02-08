using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using UnityEngine.Collections;
using Gungeon;
using Dungeonator;
using System.Collections;

namespace Swordtress
{
    class BerserkersWarCry : PassiveItem
    {
        public static int Init()
        {
            string itemName = "Berserkers war cry";
            string resourceName = "Swordtress/Resources/angryVikingMan.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<BerserkersWarCry>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "cower in fear you fragile blades!";
            string longDesc = "50% chance to terrify enemies when you kill an enemy";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            item.quality = PickupObject.ItemQuality.C;
            return item.PickupObjectId;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            m_player = player;
            if (fleeData == null || fleeData.Player != player)
            {
                fleeData = new FleePlayerData();
                fleeData.Player = player;
                fleeData.StartDistance = 100f;
            }
            player.OnDealtDamageContext += this.OnDidDamage;
        }        
        public override DebrisObject Drop(PlayerController player)
        {
            m_player.OnDealtDamageContext -= OnDidDamage;
            return base.Drop(player);
        }
        protected override void OnDestroy()
        {
            m_player.OnDealtDamageContext -= OnDidDamage;
            base.OnDestroy();
        }
        public void OnDidDamage(PlayerController player, float damage, bool fatal, HealthHaver target)
        {
            if (fatal)
            {
                List<AIActor> enemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                if (UnityEngine.Random.Range(0f, 1f) > (1f - 0.50))// is a 50% chance
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].behaviorSpeculator != null)
                        {
                            enemies[i].behaviorSpeculator.FleePlayerData = fleeData;
                            FleePlayerData fleePlayerData = new FleePlayerData();
                            GameManager.Instance.StartCoroutine(RemoveFear(enemies[i], 7));
                        }
                    }
                }
            }            
        }
        private static IEnumerator RemoveFear(AIActor aiactor, float timeTillRemoval)
        {
            yield return new WaitForSeconds(timeTillRemoval);
            aiactor.behaviorSpeculator.FleePlayerData = null;
            yield break;
        }
        PlayerController m_player;
        private static FleePlayerData fleeData;
    }
}
