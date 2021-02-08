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
    class RoseCharm : PassiveItem
    {
        public static void Init()
        {
            string itemName = "rose charm";
            string resourceName = "Swordtress/Resources/rose.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<RoseCharm>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "such beauty!";
            string longDesc = "no one wants to hit a beatiful rose. enemies that try to hit you have a 50% chance to be charmed instead.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            item.quality = PickupObject.ItemQuality.A;
        }
        public override void Pickup(PlayerController player)
        {
            m_player = player;
            base.Pickup(player);
            player.specRigidbody.OnPreRigidbodyCollision += this.OnPreCollision;
        }
        GameActorCharmEffect charm = (PickupObjectDatabase.GetById(527) as BulletStatusEffectItem).CharmModifierEffect;
        public void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            {
                if (otherRigidbody.aiActor != null)
                {
                    otherRigidbody.aiActor.ApplyEffect(charm);
                }

                if (otherRigidbody.projectile != null && otherRigidbody.projectile.Owner != null)
                {
                    otherRigidbody.projectile.Owner.ApplyEffect(charm);
                }

            }         
        }
        PlayerController m_player;
    }
}
