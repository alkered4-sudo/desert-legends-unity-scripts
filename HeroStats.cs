using UnityEngine;

[CreateAssetMenu(fileName = "NewHeroStats", menuName = "AbtalAlRimal/Hero Stats")]
public class HeroStats : ScriptableObject
{
    public string heroName;
        public string kingdom;
            [TextArea] public string abilityDescription;

                public float maxHp = 150f;
                    public float moveSpeed = 4.5f;
                        public float attackDamage = 18f;
                            public float attackRange = 2.5f;
                                public float attackCooldown = 0.8f;

                                    public string specialName;
                                        public float specialCooldown = 6f;
                                            public float specialRadius = 3.5f;
                                                public float specialDamage = 25f;
                                                    public AbilityType specialType;

                                                        [HideInInspector] public float speedMultiplier = 1f;

                                                            public float CurrentMoveSpeed => moveSpeed * speedMultiplier;
                                                            }

public enum AbilityType
{
    GroundSlam,
        ShadowDash,
            AreaBlast,
                MultiShot
                }

// Seif Al-Lahab (Fire Kingdom): HP175 Speed4.2 Dmg18 Range2.2 GroundSlam
// Amir Al-Sawaeq (Lightning Kingdom): HP82 Speed6.0 Dmg9 Range1.8 ShadowDash
// Sayyid Al-Rimal (Sand Kingdom): HP86 Speed4.0 Dmg25 Range4.0 AreaBlast
// Saqr Al-Sama (Wind Kingdom): HP98 Speed4.8 Dmg11 Range5.5 MultiShot
// Haris Al-Jaheem (Lava Guardians): HP210 Speed3.5 Dmg14 Range2.3 GroundSlam
