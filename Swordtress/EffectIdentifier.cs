using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swordtress
{
    public class EffectIdentifier
    {
        public static GameActorHealthEffect PoisonEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
        public static GameActorFreezeEffect FreezeEffect = (PickupObjectDatabase.GetById(402) as Gun).DefaultModule.projectiles[0].freezeEffect;
        public static GameActorFireEffect FireEffect = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
        public static GameActorCharmEffect CharmEffect = Gungeon.Game.Items["charming_rounds"].GetComponent<BulletStatusEffectItem>().CharmModifierEffect;
        public static GameActorSpeedEffect SlowEffect = (ETGMod.Databases.Items["triple_crossbow"] as Gun).DefaultModule.projectiles[0].speedEffect;
    }
}
