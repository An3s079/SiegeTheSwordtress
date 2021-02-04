using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace Swordtress
{
    class ThiefBomb : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Thieves Bomb";
            string resourceName = "Swordtress/Resources/ThiefBomb.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ThiefBomb>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "BOOM!";
            string longDesc = "Used by the thief to make daring escapes and leaves a wake of flames upon its desctruction.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 35f);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        protected override void DoEffect(PlayerController user)
        {

            AkSoundEngine.PostEvent("Play_BOSS_Punchout_Swing_Right_01", base.gameObject);
            Projectile projectile = ((Gun)ETGMod.Databases.Items[19]).DefaultModule.projectiles[0];
            Vector3 vector = user.unadjustedAimPoint - user.LockedApproximateSpriteCenter;
            Vector3 vector2 = user.specRigidbody.UnitCenter;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = user;
                component.Shooter = user.specRigidbody;
                component.OnDestruction += this.HandleDestruction;
            }
            
        }
        private void HandleDestruction(Projectile sourceProjectile)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopthatworks.asset");
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(sourceProjectile.LastPosition, 3f, 1f, false);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
    }
}
