using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Swordtress
{
   public static class ExploderExtensions
    {
        
        public static void DoRadialDamageNoIFrames(float damage, Vector3 position, float radius, bool damagePlayers, bool damageEnemies, bool ignoreDamageCaps = false, VFXPool hitVFX = null)
        {
            List<HealthHaver> allHealthHavers = StaticReferenceManager.AllHealthHavers;
            if (allHealthHavers != null)
            {
                for (int i = 0; i < allHealthHavers.Count; i++)
                {
                    HealthHaver healthHaver = allHealthHavers[i];
                    if (healthHaver)
                    {
                        if (healthHaver.gameObject.activeSelf)
                        {
                            if (!healthHaver.aiActor || !healthHaver.aiActor.IsGone)
                            {
                                if (!healthHaver.aiActor || healthHaver.aiActor.isActiveAndEnabled)
                                {
                                    for (int j = 0; j < healthHaver.NumBodyRigidbodies; j++)
                                    {
                                        SpeculativeRigidbody bodyRigidbody = healthHaver.GetBodyRigidbody(j);
                                        Vector2 a = healthHaver.transform.position.XY();
                                        Vector2 vector = a - position.XY();
                                        bool flag = false;
                                        bool flag2 = false;
                                        float num;
                                        if (bodyRigidbody.HitboxPixelCollider != null)
                                        {
                                            a = bodyRigidbody.HitboxPixelCollider.UnitCenter;
                                            vector = a - position.XY();
                                            num = BraveMathCollege.DistToRectangle(position.XY(), bodyRigidbody.HitboxPixelCollider.UnitBottomLeft, bodyRigidbody.HitboxPixelCollider.UnitDimensions);
                                        }
                                        else
                                        {
                                            a = healthHaver.transform.position.XY();
                                            vector = a - position.XY();
                                            num = vector.magnitude;
                                        }
                                        if (num < radius)
                                        {
                                            PlayerController component = healthHaver.GetComponent<PlayerController>();
                                            if (component != null)
                                            {
                                                bool flag3 = true;
                                                if (PassiveItem.ActiveFlagItems.ContainsKey(component) && PassiveItem.ActiveFlagItems[component].ContainsKey(typeof(HelmetItem)) && num > radius * HelmetItem.EXPLOSION_RADIUS_MULTIPLIER)
                                                {
                                                    flag3 = false;
                                                }
                                                if (damagePlayers && flag3 && !component.IsEthereal)
                                                {
                                                    HealthHaver healthHaver2 = healthHaver;
                                                    float damage2 = 0.5f;
                                                    Vector2 direction = vector;
                                                    string enemiesString = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
                                                    CoreDamageTypes damageTypes = CoreDamageTypes.None;
                                                    DamageCategory damageCategory = DamageCategory.Normal;
                                                    healthHaver2.ApplyDamage(damage2, direction, enemiesString, damageTypes, damageCategory, false, null, ignoreDamageCaps);
                                                    flag2 = true;
                                                }
                                            }
                                            else if (damageEnemies)
                                            {
                                                AIActor aiActor = healthHaver.aiActor;
                                                if (damagePlayers || !aiActor || aiActor.IsNormalEnemy)
                                                {
                                                    HealthHaver healthHaver3 = healthHaver;
                                                    Vector2 direction = vector;
                                                    string enemiesString = StringTableManager.GetEnemiesString("#EXPLOSION", -1);
                                                    CoreDamageTypes damageTypes = CoreDamageTypes.None;
                                                    DamageCategory damageCategory = DamageCategory.Normal;
                                                    healthHaver3.ApplyDamage(damage, direction, enemiesString, damageTypes, damageCategory, true, null, ignoreDamageCaps);
                                                    flag2 = true;
                                                }
                                            }
                                            flag = true;
                                        }
                                        if (flag2 && hitVFX != null)
                                        {
                                            if (bodyRigidbody.HitboxPixelCollider != null)
                                            {
                                                PixelCollider pixelCollider = bodyRigidbody.GetPixelCollider(ColliderType.HitBox);
                                                Vector2 v = BraveMathCollege.ClosestPointOnRectangle(position, pixelCollider.UnitBottomLeft, pixelCollider.UnitDimensions);
                                                hitVFX.SpawnAtPosition(v, 0f, null, null, null, null, false, null, null, false);
                                            }
                                            else
                                            {
                                                hitVFX.SpawnAtPosition(healthHaver.transform.position.XY(), 0f, null, null, null, null, false, null, null, false);
                                            }
                                        }
                                        if (flag)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

