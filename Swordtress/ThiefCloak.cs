using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using System.Collections;

namespace Swordtress
{
    class ThiefCloak : PassiveItem
    {
        public static int Init()
        {
            string itemName = "Thieves Cloak";
            string resourceName = "Swordtress/Resources/ThiefCloak.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ThiefCloak>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Into the Shadows.";
            string longDesc = "Upon entering a room full of enemies you are given a brief moment of time to position yourself.\n\n";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Coolness, 1);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            return item.PickupObjectId;
        }

        private bool isStealth;
        public void OnEnteredCombat()
        {
            if (isStealth == false)
            {
                isStealth = true;
                AlterItemStats.AddStatToPassive(this, PlayerStats.StatType.MovementSpeed, 1.4f, StatModifier.ModifyMethod.MULTIPLICATIVE);
                Owner.stats.RecalculateStats(Owner, false, false);
                this.HandleStealth(GameManager.Instance.PrimaryPlayer);
            }
        }
        private void HandleStealth(PlayerController player)
        {
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
            player.ChangeSpecialShaderFlag(1, 1f);
            player.SetIsStealthed(true, "smoke");
            player.SetCapableOfStealing(true, "StealthItem", null);
            player.specRigidbody.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));

            player.OnItemStolen += this.BreakStealthOnSteal;
            StartCoroutine(WaitaSec(player));
        }
        IEnumerator WaitaSec(PlayerController player)
        {
            player.OnDidUnstealthyAction += this.BreakStealth;
            yield return new WaitForSecondsRealtime(5);
            BreakStealth(player);
        }

        public void BreakStealth(PlayerController player)
        {
            isStealth = false;
            player.OnDidUnstealthyAction -= this.BreakStealth;
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.specRigidbody.RemoveCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox, CollisionLayer.EnemyCollider));
            player.ChangeSpecialShaderFlag(1, 0f);
            player.SetIsStealthed(false, "smoke");
            player.SetCapableOfStealing(false, "StealthItem", null);
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
            AlterItemStats.RemoveStatFromPassive(this, PlayerStats.StatType.MovementSpeed);
            Owner.stats.RecalculateStats(Owner, false, false);
        }
        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
        {
            this.BreakStealth(arg1);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += OnEnteredCombat;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= OnEnteredCombat;
            return base.Drop(player);
        }
    }
}
