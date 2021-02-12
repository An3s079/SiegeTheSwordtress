using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace Swordtress
{
    public class Zenith : GunBehaviour
    {
        public static int Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Zenith", "zenith");

            Game.Items.Rename("outdated_gun_mods:zenith", "sts:zenith");
            gun.gameObject.AddComponent<Zenith>();

            gun.SetShortDescription("short desc");
            gun.SetLongDescription("long desc");

            gun.SetupSprite(null, "zenith_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 2);
            gun.shootAnimation = gun.UpdateAnimation("fire", null, false);
            gun.muzzleFlashEffects = null;

            gun.AddProjectileModuleFrom("ak-47", true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.06f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.SetBaseMaxAmmo(800);

            gun.quality = PickupObject.ItemQuality.S;
            gun.encounterTrackable.EncounterGuid = "'xilucvvvvvvvvvvvvvvvvvvhriorjboiuhbnribb  hjk gbjhfg bf f dkfjnsdkjvdfiuvd' -spcreat, 2020";

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage *= 1.0f;
            projectile.baseData.speed *= 0f;
            projectile.transform.parent = gun.barrelOffset;
            projectile.sprite.renderer.enabled = false;

            projectile.SetProjectileSpriteRight("zenith_projectile_001", 29, 10, null, null);
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            return gun.PickupObjectId;

        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            base.OnPostFired(player, gun);
            gun.PreventNormalFireAudio = true;
            if (gun.CurrentAmmo % 3 == 0)
            {
                AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", gameObject);
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            PierceProjModifier pierceProjModifier = projectile.GetComponent<PierceProjModifier>();
            if (pierceProjModifier == null)
            {
                pierceProjModifier = base.gameObject.AddComponent<PierceProjModifier>();
                pierceProjModifier.penetration = int.MaxValue;
                pierceProjModifier.penetratesBreakables = true;
                pierceProjModifier.BeastModeLevel = PierceProjModifier.BeastModeStatus.NOT_BEAST_MODE;
            }
            else
            {
                pierceProjModifier.penetration = int.MaxValue;
            }
            projectile.IgnoreTileCollisionsFor(10f);
            projectile.OverrideMotionModule = new ZenithProjectile(ResourceExtractor.GetTextureFromResource("ExampleMod/Resources/squaregrad.png"));

            var rot = projectile.Owner.FacingDirection + 180f;
            projectile.transform.eulerAngles = new Vector3(0f, 0f, rot);
            projectile.transform.position = projectile.Owner.CenterPosition;

            //StartCoroutine(HandleProjectileMovement(projectile));
            //projectile.OnPostUpdate += Projectile_OnPostUpdate;

            base.PostProcessProjectile(projectile);
        }

        public class ZenithProjectile : ProjectileMotionModule
        {
            Texture2D _gradTexture;

            public ZenithProjectile(Texture2D gradTexture)
            {
                _gradTexture = gradTexture;
            }

            public override void UpdateDataOnBounce(float angleDiff)
            {
                //throw new NotImplementedException();
            }

            bool setup = false;
            float initialRot = 0f;
            Vector2 targetPosition;
            Vector2 startPosition;

            float startOpacity = 0f;
            float additionalMagnitude = 0f;
            float additionalMagnitude2 = 0f;
            int direction = 1;

            TrailRenderer tr;

            public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
            {
                if (!setup)
                {
                    setup = true;
                    //Tools.Print(source.Owner.FacingDirection);
                    initialRot = source.Owner.FacingDirection;
                    startPosition = source.Owner.CenterPosition;
                    source.transform.position = startPosition;
                    startOpacity = UnityEngine.Random.Range(0.3f, 1f);
                    additionalMagnitude = UnityEngine.Random.Range(-2f, 2f);
                    additionalMagnitude2 = UnityEngine.Random.Range(0.7f, 1.3f);
                    direction = UnityEngine.Random.value < 0.5 ? -1 : 1;
                    var sprite = UnityEngine.Random.value < 0.35f ? UnityEngine.Random.Range(2, 8) : 1;
                    source.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName("zenith_projectile_00" + sprite);
                    ResetTargetPosition(source);

                    source.sprite.renderer.enabled = true;

                    var tro = source.gameObject.AddChild("trail object");
                    tro.transform.position = source.transform.position;
                    tro.transform.localPosition = new Vector3(1f, 0.2f, 0f);

                    tr = tro.AddComponent<TrailRenderer>();
                    tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    tr.receiveShadows = false;
                    var mat = new Material(Shader.Find("Sprites/Default"));
                    mat.mainTexture = _gradTexture;
                    mat.SetColor("_Color", GetColorFromSprite(sprite));
                    tr.material = mat;
                    tr.time = 0.2f;
                    tr.minVertexDistance = 0.1f;
                    tr.startWidth = 1f;
                    tr.endWidth = 0f;
                    tr.startColor = Color.white;
                    tr.endColor = new Color(1f, 1f, 1f, 0f);
                }
                ResetTargetPosition(source);
                //throw new NotImplementedException();
                //projectileTransform.Rotate(new Vector3(0f, 0f, 100f * BraveTime.DeltaTime));
                //source.r
                var rot = initialRot + (m_timeElapsed * 720f * direction) + 180f;
                source.transform.eulerAngles = new Vector3(0f, 0f, rot);
                float targetMagnitude = 0f;
                if (m_timeElapsed <= 0.25f)
                {
                    //targetPosition = source.Owner.CenterPosition;
                    startPosition = source.Owner.CenterPosition;
                }
                else
                {
                    //targetMagnitude = Mathf.Lerp(0f, ((source.Owner.CenterPosition) - targetPosition).magnitude, (float)Math.Sin(m_timeElapsed * Math.PI * 2));
                }
                targetMagnitude = Mathf.Lerp(0f, (targetPosition - startPosition).magnitude, (float)Math.Sin(m_timeElapsed * Math.PI * 2));
                float targetSidenitude = (float)Math.Sin(m_timeElapsed * Math.PI * 4) * 1.3f * additionalMagnitude2 * (float)(direction * -1);
                //targetSidenitude = 0f;
                //targetMagnitude = 2f;
                var opac = Mathf.Clamp01((float)Math.Sin(m_timeElapsed * Math.PI * 2) * 2f) * startOpacity;
                tr.startColor = new Color(1f, 1f, 1f, opac * 0.5f);
                source.ChangeColor(0f, new Color(opac, opac, opac));
                source.transform.position = startPosition + new Vector2(targetMagnitude + additionalMagnitude, targetSidenitude).Rotate(initialRot);
                ExploderExtensions.DoRadialDamageNoIFrames(0.15f, source.transform.position, 3f, false, true);
                //source.specRigidbody.Velocity = new Vector2(10f, 0f).Rotate(rot+90f);
                if (m_timeElapsed > 0.5f)
                {
                    source.ForceDestruction();
                }
                m_timeElapsed += (BraveTime.DeltaTime);
            }

            Color GetColorFromSprite(int ind)
            {
                switch (ind)
                {
                    case 2:
                        return new Color32(242, 244, 245, 255);
                    case 3:
                        return new Color32(211, 212, 184, 255);
                    case 4:
                        return new Color32(255, 140, 140, 255);
                    case 5:
                        return new Color32(135, 135, 135, 255);
                    case 6:
                        return new Color32(255, 254, 219, 255);
                    case 7:
                        return new Color32(255, 181, 102, 255);
                    default:
                        return new Color32(137, 215, 232, 255);
                }
            }

            void ResetTargetPosition(Projectile source)
            {
                if (source.Owner is PlayerController player)
                {
                    targetPosition = player.unadjustedAimPoint.XY();
                }
                else if (source.Owner is AIActor ai)
                {
                    targetPosition = ai.PlayerTarget.CenterPosition;
                }
                else
                {
                    targetPosition = new Vector2(10f, 0f).Rotate(initialRot);
                }
            }
        }

        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
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
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }
    }
}
