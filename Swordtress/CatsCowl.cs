using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
namespace Swordtress
{
    class CatsCowl : PegasusBootsItem
    {
        public static void Init()
        {
            string itemName = "Black Cats Cowl";
            string resourceName = "Swordtress/Resources/black_cats_cowl.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<PegasusBootsItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Its Like Walking On Air";
            string longDesc = "Gives its owner increased movement and control in combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "sts");
            item.AddPassiveStatModifier(PlayerStats.StatType.DodgeRollSpeedMultiplier, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = PickupObject.ItemQuality.B;
        }
    }
}