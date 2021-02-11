using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Swordtress
{
    class DyingPan : AdvancedGunBehaviour
    {
        public static int Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Dying Pan", "dying_pan");
            Game.Items.Rename("outdated_gun_mods:dying_pan", "sts:dying_pan");
            gun.gameObject.AddComponent<DyingPan>();
            gun.SetShortDescription("sizzle~");
            gun.SetLongDescription("I'll use my trusty frying pan, as a dying pan.");
            gun.SetupSprite(null, "dying_pan_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            gun.SetAnimationFPS(gun.reloadAnimation, 2);
            gun.AddProjectileModuleFrom("38_special", true, false);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SMALL_BULLET;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.cooldownTime = .6f;
            gun.DefaultModule.numberOfShotsInClip = 250;
            Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetBaseMaxAmmo(350);
            gun.barrelOffset.transform.localPosition = new Vector3(0.75f, 0f, 0f);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "what the hell even is this item i cant believe im actually making this shit.";
            gun.sprite.IsPerpendicular = true;
            gun.gunClass = GunClass.NONE;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage = 6.5f;
            ETGModConsole.Log($"{projectile.baseData.damage}");
            projectile.AppliesFire = true;
            projectile.FireApplyChance = 0.35f;
            projectile.fireEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
            VFXPool PanSlashVFX = VFXLibrary.CreateMuzzleflash("royal_guard's_knife_slash", new List<string> { "royal_guard's_knife_slash_001", "royal_guard's_knife_slash_002", "royal_guard's_knife_slash_003", "royal_guard's_knife_slash_004", }, 10, new List<IntVector2> { new IntVector2(23, -5), new IntVector2(23, -5), new IntVector2(23, -5), new IntVector2(23, -5), }, new List<tk2dBaseSprite.Anchor> {
                tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft, tk2dBaseSprite.Anchor.MiddleLeft}, new List<Vector2> { new Vector2(5, 0), new Vector2(5, 0), new Vector2(5, 0), new Vector2(5, 0), }, false, false, false, false, 0, VFXAlignment.Fixed, true, new List<float> { 0, 0, 0, 0 }, new List<Color> { VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, VFXLibrary.emptyColor, });

            ProjectileSlashingBehaviour slashingBehaviour = projectile.gameObject.AddComponent<ProjectileSlashingBehaviour>();
            slashingBehaviour.SlashVFX = PanSlashVFX;
            slashingBehaviour.SlashDimensions = 65;
            slashingBehaviour.SlashRange = 2.5f;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1);
            gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1);
            return gun.PickupObjectId;
        }
        private bool HasReloaded;

        protected override void Update()
        {
            if (gun.CurrentOwner)
            {

                if (gun.PreventNormalFireAudio)
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
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);

            }
        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {

            gun.PreventNormalFireAudio = true;

        }



        public DyingPan()
        {

        }
    }
}
