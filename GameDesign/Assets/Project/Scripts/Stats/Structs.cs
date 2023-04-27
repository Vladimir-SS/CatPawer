using Stats.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stats
{
    namespace Structs
    {
        [Serializable]
        public struct Body
        {
            [Tooltip("Amount of damage you can take before dying")]
            public float MaxHP;

            [Tooltip("You reduce incoming damage by x%. Each point of armor means it takes 6.66% more damage to kill you.")]
            public float Armor;

            [Tooltip("You move x% faster.")]
            public float Speed;

            public static Body operator+ (Body left, Body right)
            {
                return new Body
                {
                    Armor = left.Armor + right.Armor,
                    MaxHP = left.MaxHP + right.MaxHP,
                    Speed = left.Speed + right.Speed
                };
            }

            public static Body operator- (Body left, Body right)
            {
                return new Body
                {
                    Armor = left.Armor - right.Armor,
                    MaxHP = left.MaxHP - right.MaxHP,
                    Speed = left.Speed - right.Speed
                };
            }
        }




        

        [Serializable]
        public struct Combat
        {
            [Tooltip("Your attacks deal x% more damage")]
            public float DamageBonus;

            [Tooltip("Your attacks deal x additional damage")]
            public float DamageBase;
            
            public static Combat operator+ (Combat left, Combat right)
            {
                return new Combat
                {
                    DamageBase = left.DamageBase + right.DamageBase,
                    DamageBonus = left.DamageBonus + right.DamageBonus
                };
            }

            public static Combat operator- (Combat left, Combat right)
            {
                return new Combat
                {
                    DamageBase = left.DamageBase - right.DamageBase,
                    DamageBonus = left.DamageBonus - right.DamageBonus
                };
            }
        }

        [Serializable]
        public struct Gun
        {
            [Tooltip("TODO: add description")]
            public float FireRate;

            [Tooltip("TODO: add description")]
            public float ReloadSpeed;

            [Tooltip("TODO: add description")]
            public uint MagazineCapacity;

            public static Gun operator+ (Gun left, Gun right)
            {
                return new Gun
                {
                    FireRate = left.FireRate + right.FireRate,
                    ReloadSpeed = left.ReloadSpeed + right.ReloadSpeed,
                    MagazineCapacity = left.MagazineCapacity + right.MagazineCapacity
                };
            }

            public static Gun operator- (Gun left, Gun right)
            {
                return new Gun
                {
                    FireRate = left.FireRate - right.FireRate,
                    ReloadSpeed = left.ReloadSpeed - right.ReloadSpeed,
                    MagazineCapacity = left.MagazineCapacity - right.MagazineCapacity
                };
            }
        }

        namespace Final
        {
            [Serializable]
            public struct Body
            {
                public float MaxHP;
                public float DamageReduction;
                public float Speed;

                private static float CalculateDamageReductionAux(float armor)
                {
                    return 1 - 10 / (10 + armor);
                }

                private static float CalculateDamageReduction(float armor)
                {
                    if (armor > 0)
                        return CalculateDamageReductionAux(armor);
                    return -CalculateDamageReductionAux(-armor);
                }


                public static implicit operator Body(Structs.Body body)
                {
                    return new Body
                    {
                        MaxHP = Math.Max(1, body.MaxHP),
                        Speed = body.Speed,
                        DamageReduction = CalculateDamageReduction(body.Armor)
                    };
                }
            }

            [Serializable]
            public struct Combat
            {
                public float Damage;

                public static implicit operator Combat(Structs.Combat combat)
                {
                    return new Combat
                    {
                        Damage = combat.DamageBase + combat.DamageBase * (combat.DamageBonus / 100)
                    };
                }
            }
        }
    }
}

