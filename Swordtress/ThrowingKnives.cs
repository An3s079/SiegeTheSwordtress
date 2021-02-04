using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace Swordtress
{
    class ThrowingKnives : AdvancedGunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Throwing Knives", "throwingknives");
            Game.Items.Rename("outdated_gun_mods:throwing_knives", "sts:throwing_knives");
            gun.gameObject.AddComponent<ThrowingKnives>();
            gun.SetShortDescription("precision needed.");
            gun.SetLongDescription("Although this weapon is a knife, it is thrown as a projectile, and projectiles anger the dulled.");
            gun.SetupSprite(null, "throwingknives_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.gunHandedness = GunHandedness.OneHanded;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = int.MaxValue;
            gun.SetBaseMaxAmmo(300);
            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "haha throw go brrrrrr in mod thats not supposed to be about swords.";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.barrelOffset.transform.localPosition = new Vector3(0f, 0f, 0f);
            projectile.baseData.damage = 5f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.SetProjectileSpriteRight("throwingknives_projectile_001", 15, 10, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            projectile.shouldRotate = true;
            projectile.angularVelocity = 400;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
        }
        private bool HasReloaded;
        protected void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
        }
    }
}
