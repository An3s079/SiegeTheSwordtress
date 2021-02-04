using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Gungeon;
using System.Collections;
namespace Swordtress
{
    public class ProjectileSlashingBehaviour : MonoBehaviour
    {
        public ProjectileSlashingBehaviour()
        {
            DestroyBaseAfterFirstSlash = true;
            timeBetweenSlashes = 1;
            DoSound = true;
            slashKnockback = 5;
            SlashDamage = 15;
            SlashBossMult = 1;
            SlashJammedMult = 1;
            playerKnockback = 1;
            SlashDamageUsesBaseProjectileDamage = true;
            InteractMode = SlashDoer.ProjInteractMode.IGNORE;
            SlashDimensions = 90;
            SlashRange = 2.5f;
            SlashVFX = (ETGMod.Databases.Items["wonderboy"] as Gun).muzzleFlashEffects;
            soundToPlay = "Play_WPN_blasphemy_shot_01";
            DoesMultipleSlashes = false;
            UsesAngleVariance = false;
            MinSlashAngleOffset = 1;
            MaxSlashAngleOffset = 4;
            delayBeforeSlash = 0;
        }
        private void Start()
        {
            this.m_projectile = base.GetComponent<Projectile>();
            if (this.m_projectile.Owner && this.m_projectile.Owner is PlayerController) this.owner = this.m_projectile.Owner as PlayerController;
            this.m_projectile.sprite.renderer.enabled = false;
            this.m_projectile.collidesWithEnemies = false;
            this.m_projectile.UpdateCollisionMask();
            if (this.m_projectile)
            {
                if (doSpinAttack)
                {
                    DestroyBaseAfterFirstSlash = false;
                    this.m_projectile.StartCoroutine(DoSlash(90, 0.15f + delayBeforeSlash));
                    this.m_projectile.StartCoroutine(DoSlash(180, 0.30f + delayBeforeSlash));
                    this.m_projectile.StartCoroutine(DoSlash(-90, 0.45f + delayBeforeSlash));
                    Invoke("Suicide", 0.01f);
                }
                else if (DoesMultipleSlashes)
                {
                    this.m_projectile.StartCoroutine(DoMultiSlash(0, delayBeforeSlash, AmountOfMultiSlashes, DelayBetweenMultiSlashes));
                }
                else
                {
                    this.m_projectile.StartCoroutine(DoSlash(0, 0 + delayBeforeSlash));
                }
            }
        }
        private void Update()
        {


        }
        private IEnumerator DoSlash(float angle, float delay)
        {
            yield return new WaitForSeconds(delay);
            float actDamage = this.SlashDamage;
            float actKnockback = this.slashKnockback;
            float bossDMGMult = this.SlashBossMult;
            float jammedDMGMult = this.SlashJammedMult;

            if (SlashDamageUsesBaseProjectileDamage)
            {
                actDamage = this.m_projectile.baseData.damage;
                bossDMGMult = this.m_projectile.BossDamageMultiplier;
                jammedDMGMult = this.m_projectile.BlackPhantomDamageMultiplier;
                actKnockback = this.m_projectile.baseData.force;
            }
            if (UsesAngleVariance)
            {
                angle += UnityEngine.Random.Range(MinSlashAngleOffset, MaxSlashAngleOffset);
            }
            SlashDoer.DoSwordSlash(this.m_projectile.specRigidbody.UnitCenter, (this.m_projectile.Direction.ToAngle() + angle), owner, playerKnockback, this.InteractMode, actDamage, actKnockback, this.m_projectile.statusEffectsToApply, null, jammedDMGMult, bossDMGMult, SlashRange, SlashDimensions);
            if (DoSound) AkSoundEngine.PostEvent(soundToPlay, this.m_projectile.gameObject);
            SlashVFX.SpawnAtPosition(this.m_projectile.specRigidbody.UnitCenter, this.m_projectile.Direction.ToAngle() + angle, null, null, null, -0.05f);
            if (DestroyBaseAfterFirstSlash) Suicide();
            yield break;
        }
        private IEnumerator DoMultiSlash(float angle, float delay, int AmountOfMultiSlashes, float DelayBetweenMultiSlashes)
        {
            yield return new WaitForSeconds(delay);
            float actDamage = this.SlashDamage;
            float actKnockback = this.slashKnockback;
            float bossDMGMult = this.SlashBossMult;
            float jammedDMGMult = this.SlashJammedMult;
            for (int i = 0; i < AmountOfMultiSlashes; i++)
            {
                if (SlashDamageUsesBaseProjectileDamage)
                {
                    actDamage = this.m_projectile.baseData.damage;
                    bossDMGMult = this.m_projectile.BossDamageMultiplier;
                    jammedDMGMult = this.m_projectile.BlackPhantomDamageMultiplier;
                    actKnockback = this.m_projectile.baseData.force;
                }
                if (UsesAngleVariance)
                {
                    angle += UnityEngine.Random.Range(MinSlashAngleOffset, MaxSlashAngleOffset);
                }
                SlashDoer.DoSwordSlash(this.m_projectile.specRigidbody.UnitCenter, (this.m_projectile.Direction.ToAngle() + angle), owner, playerKnockback, this.InteractMode, actDamage, actKnockback, this.m_projectile.statusEffectsToApply, null, jammedDMGMult, bossDMGMult, SlashRange, SlashDimensions);
                if (DoSound) AkSoundEngine.PostEvent(soundToPlay, this.m_projectile.gameObject);
                SlashVFX.SpawnAtPosition(this.m_projectile.specRigidbody.UnitCenter, this.m_projectile.Direction.ToAngle() + angle, null, null, null, -0.05f);
                yield return new WaitForSeconds(DelayBetweenMultiSlashes);
            }
            Suicide();
            yield break;
        }
        private void Suicide() { UnityEngine.Object.Destroy(this.m_projectile.gameObject); }
        private float timer;
        /// <summary>
        /// The sound that will play when the slash goes off. You can set this to be a custom sound. If not set, it will use Blasphemy's instead. If you are using this, make sure to have the weapon's PreventNormalFireAudio to false! 
        /// </summary>
        public string soundToPlay;
        /// <summary>
        /// The delay before the first slash
        /// </summary>
        public float delayBeforeSlash;
        /// <summary>
        /// The VFX of a slash. Use CreateMuzzleFlash in VFXLibrary to make a custom one. Uses Blasphemy's if not set. If you make a custom slash vfx, place the sprites in a folder called VFXCollection, inside of your sprites folder (the one that has weapon sprites)
        /// </summary>
        public VFXPool SlashVFX;
        /// <summary>
        /// If you want to do something like Katana Bullets I guess
        /// </summary>
        public float timeBetweenSlashes;
        /// <summary>
        /// If the weapon does a spin attack
        /// </summary>
        public bool doSpinAttack;
        /// <summary>
        /// Don't know what this does. Best to not change it
        /// </summary>
        public float playerKnockback;
        /// <summary>
        /// Knockback of the slash. Doesn't need to be set if SlashDamageUsesBaseProjectileDamage is true
        /// </summary>
        public float slashKnockback;
        /// <summary>
        /// If there is a sound when the slash goes off.
        /// </summary>
        public bool DoSound;
        /// <summary>
        /// The jammed damage multiplier of the slash. Doesn't need to be set if SlashDamageUsesBaseProjectileDamage is true
        /// </summary>
        public float SlashJammedMult;
        /// <summary>
        /// The boss damage multiplier of the slash. Doesn't need to be set if SlashDamageUsesBaseProjectileDamage is true
        /// </summary>
        public float SlashBossMult;
        /// <summary>
        /// The damage of the slash. Doesn't need to be set if SlashDamageUsesBaseProjectileDamage is true
        /// </summary>
        public float SlashDamage;
        /// <summary>
        /// How far a slash will damage the enemies from the point of it being spawned
        /// </summary>
        public float SlashRange;
        /// <summary>
        /// The angle width of a slash
        /// </summary>
        public float SlashDimensions;
        /// <summary>
        /// If the slash uses the base stats of the projectile it is attached to. Best left true
        /// </summary>
        public bool SlashDamageUsesBaseProjectileDamage;
        /// <summary>
        /// How the slash interacts with enemy projectiles
        /// </summary>
        public SlashDoer.ProjInteractMode InteractMode;
        /// <summary>
        /// If the project is destroyed after the first slash or not. Is automatically set to false if DoesMultipleSlashes or doSpinAttack is set to true
        /// </summary>
        public bool DestroyBaseAfterFirstSlash;
        /// <summary>
        /// Allows the weapon to do a burst of multiple slashes
        /// </summary>
        public bool DoesMultipleSlashes;
        /// <summary>
        /// The minimum angle offset a slash can have
        /// </summary>
        public float MinSlashAngleOffset;
        /// <summary>
        /// The maximum angle offset a slash can have
        /// </summary>
        public float MaxSlashAngleOffset;
        /// <summary>
        /// If the slash will have an angle offset from the aim of the player
        /// </summary>
        public bool UsesAngleVariance;
        /// <summary>
        /// Amount of slashes in a burst
        /// </summary>
        public int AmountOfMultiSlashes;
        /// <summary>
        /// Determines how much time is between each slash in a burst
        /// </summary>
        public float DelayBetweenMultiSlashes;
        private Projectile m_projectile;
        private PlayerController owner;
    }
}
