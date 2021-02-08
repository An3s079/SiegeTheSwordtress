using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace Swordtress
{
    class CritItem : PassiveItem
    {
            public static int Init()
            {
                string itemName = "Berserkers Brew";
                string resourceName = "Swordtress/Resources/berserkers_brew.png";
                GameObject obj = new GameObject();
                CritItem item = obj.AddComponent<CritItem>();
                ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
                string shortDesc = "It did not taste good.";
                string longDesc = "A brew made by Siegatures to give extra strength to whomever drank from it. \n\n" + 
                "Sadly, decades of sitting untouched have caused it to be a mere sliver of its former glory, with its increased strength being unpredictable.";
                ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
                item.quality = ItemQuality.C;
                return item.PickupObjectId;
            }

            public override void Pickup(PlayerController player)
            {
                player.PostProcessProjectile += Player_PostProcessProjectile;
                base.Pickup(player);
            }

            private void Player_PostProcessProjectile(Projectile projectile, float arg2)
            {
                if (projectile && UnityEngine.Random.value > 0.9f)
                {
                    projectile.baseData.damage *= 2;
                    //projectile.AdjustPlayerProjectileTint(Color.red, 5);
                    projectile.OnHitEnemy += DoBlood;
                }
            }

            private void DoBlood(Projectile projectile, SpeculativeRigidbody spec, bool flag)
            {
                if (projectile && projectile.specRigidbody)
                {
                    DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(SwordtressBloodGoop).TimedAddGoopCircle(projectile.specRigidbody.UnitCenter, 2.5f);
                }
            }

            private GoopDefinition SwordtressBloodGoop = new GoopDefinition()
            {
                CanBeIgnited = false,
                damagesEnemies = false,
                damagesPlayers = false,
                baseColor32 = new Color32(207, 23, 10, 200),
                goopTexture = ResourceExtractor.GetTextureFromResource("Swordtress/Resources/goop_standard_base_001.png"),
                AppliesDamageOverTime = false,

            };
    }
}
