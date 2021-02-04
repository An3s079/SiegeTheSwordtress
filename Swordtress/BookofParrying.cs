using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swordtress
{
    class BookOfParrying : PassiveItem
    {
        public static void Init()
        {
            string ItemName = "Book of Parrying";
            string SpriteDirectory = "Swordtress/Resources/BookofParry.png";
            GameObject obj = new GameObject(ItemName);
            var item = obj.AddComponent<BookOfParrying>();
            ItemBuilder.AddSpriteToObject(ItemName, SpriteDirectory, obj);
            string shortDesc = "An essential skill for survival";
            string longDesc = "Adds a small chance to negate damage.\n\n" +
            "A book made by the legendary swordsman, containing only a single chapter and a few pages. Each page contains a single, giant letter that reads: 'learn to parry you fool!' and a smiley on the last page. Well, that's not exactly helpful, is it?";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            item.quality = PickupObject.ItemQuality.B;
        }
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            player.specRigidbody.OnPreRigidbodyCollision += this.nullifyDamage;
            
            
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.specRigidbody.OnPreRigidbodyCollision -= this.nullifyDamage;
            return base.Drop(player);
        }
        
        private void nullifyDamage(SpeculativeRigidbody mySpec, PixelCollider myPixCo, SpeculativeRigidbody otherSpec, PixelCollider otherPixCo)
        {
            if (otherSpec.projectile && UnityEngine.Random.value <= 0.1f)
            {
                otherSpec.projectile.DieInAir(false, true, true, false);
                PhysicsEngine.SkipCollision = true;
            }
        }
    }
}



