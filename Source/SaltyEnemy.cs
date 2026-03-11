using System;
using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.SaltyEnemies
{
    public static class SaltyEnemyEnemyComponentsExtension
    {
        public static SaltyEnemy GetSaltyEnemy(this EnemyComponents enemy)
        {
            return enemy.GetMonoByIndex<SaltyEnemy>(SaltyEnemy.MonoRegistrarIndex);
        }
    }

    public class SaltyEnemy : MonoBehaviour
    {
        EnemyIdentifier Eid = null;
        EnemyComponents Enemy = null;
        EnemyRadiance.Modifier RadianceModifier = new EnemyRadiance.Modifier();
        bool PlayedEnrageSound = false;
        bool EnragedByUs = false;
        float enrageSoundTimer = -1.0f;
        float EnrageSoundCooldown = -1.0f;

        public static int MonoRegistrarIndex { get; private set; } = -1;

        protected void FixedUpdate()
        {
            if (NyxLib.Cheats.IsCheatEnabled(Cheats.SaltyEnemies))
            {
                if (Eid.enemyType == EnemyType.Puppet && Eid.Dead)
                {
                    return;
                }
                
                Assert.IsNotNull(Enemy);
                Assert.IsNotNull(Enemy.PrefabStore);
                Assert.IsNotNull(Enemy.PrefabStore.Prefab);

                var prefabEid = Enemy.PrefabStore.Prefab.GetComponent<EnemyIdentifier>() ?? Enemy.PrefabStore.Prefab.GetComponentInChildren<EnemyIdentifier>();
                var radienceTier = prefabEid.hasRadianceEffected ? prefabEid.radianceTier : 0.0f;
                int rankIndex = StyleHUD.Instance.rankIndex;
                StyleRanks rank = (StyleRanks)(rankIndex);
                radienceTier += rank switch
                {
                    StyleRanks.Destructive => Options.DestructiveRadianceTier.Value,
                    StyleRanks.Chaotic => Options.ChaoticRadianceTier.Value,
                    StyleRanks.Brutal => Options.BrutalRadianceTier.Value,
                    StyleRanks.Anarchic => Options.AnarchicRadianceTier.Value,
                    StyleRanks.Supreme => Options.SupremeRadianceTier.Value,
                    StyleRanks.SSadistic => Options.SSadisticRadianceTier.Value,
                    StyleRanks.SSSensoredStorm => Options.SSSensoredStormRadianceTier.Value,
                    StyleRanks.ULTRAKILL => Options.ULTRAKILLRadianceTier.Value,
                    _ => throw new Exception("A great sadness has struck the city."),
                };
                
                if (rank is StyleRanks.ULTRAKILL)
                {
                    if (Eid.TryEnrage())
                    {
                        EnragedByUs = true;
                    }
                    else
                    {
                        radienceTier = Options.ULTRAKILLNoEnrageRadianceTier.Value;
                        enrageSoundTimer = UnityEngine.Random.value % 0.5f;
                    }
                }
                else
                {
                    if (EnragedByUs)
                    {
                        Eid.TryUnenrage();
                        EnragedByUs = false;
                    }

                    PlayedEnrageSound = false;
                    enrageSoundTimer = -1.0f;
                }

                if (radienceTier <= 0.001f)
                {
                    UnrequestBuffs();
                    return;
                }
                
                if (!Eid.dead)
                {
                    RequestBuffs(radienceTier);
                }
            }
            else
            {
                UnrequestBuffs();
            }
        }

        internal static void Initialize()
        {
            MonoRegistrarIndex = EnemyComponents.MonoRegistrar.Register<SaltyEnemy>();
        }

        private void TryPlayHuskEnrageSound(float pitch = 1.0f, float volume = 1.0f)
        {
            if (PlayedEnrageSound)
            {
                return;
            }

            if (EnrageSoundCooldown >= 0.0f)
            {
                PlayedEnrageSound = true;
                return;
            }

            if (Assets.HuskEnrageSound_0 != null)
            {
                Log.Debug($"'[SaltyEnemy] playing husk enrage sound!");
                var audioGo = GameObject.Instantiate(Assets.HuskEnrageSound_0, transform);
                audioGo.GetComponent<AudioSource>().volume *= volume;
                audioGo.GetComponent<AudioSource>().pitch = 0.3f * pitch;
                audioGo.GetComponent<AudioDistortionFilter>().distortionLevel = 0.5f;
                audioGo.SetActive(true);
                PlayedEnrageSound = true;

                EnrageSoundCooldown = 10.0f;
            }
            else
            {
                Log.Warning($"'[SaltyEnemy] Tried to play husk enrage sound but we haven't cached it yet");
            }
        }

        private void TryPlayMachineEnrageSound(float pitch = 1.0f, float volume = 1.0f)
        {
            if (PlayedEnrageSound)
            {
                return;
            }

            if (EnrageSoundCooldown >= 0.0f)
            {
                PlayedEnrageSound = true;
                return;
            }
            
            if (Assets.MachineEnrageSound_0 != null)
            {
                Log.Debug($"'[SaltyEnemy] playing machine enrage sound!");
                var audioGo = GameObject.Instantiate(Assets.MachineEnrageSound_0, transform);
                var audioSource = audioGo.GetComponent<AudioSource>();
                audioSource.volume *= volume;
                switch (Eid.enemyType)
                {
                    case EnemyType.Streetcleaner:
                    audioSource.clip = Eid.machine.scream;
                    break;
                    default:
                    var randVal = (int)(UnityEngine.Random.value * 100.0f) % 2;
                    if (randVal == 0)
                    {
                        audioSource.pitch = 2f * pitch;
                    }
                    else if (randVal == 1)
                    {
                        audioSource.clip = Eid.machine.scream;
                        audioSource.pitch = pitch;
                    }
                    break;
                }
                audioGo.SetActive(true);
                PlayedEnrageSound = true;
                EnrageSoundCooldown = 10.0f;
            }
            else
            {
                Log.Warning($"'[SaltyEnemy] Tried to play machine enrage sound but we haven't cached it yet");
            }
        }


        private void RequestBuffs(float radienceTier)
        {
            RadianceModifier.BaseEnabled = true;
            RadianceModifier.SpeedEnabled = Options.SaltEffectSpeed.Value;
            RadianceModifier.HealthEnabled = Options.SaltEffectHealth.Value;
            RadianceModifier.DamageEnabled = Options.SaltEffectDamage.Value;

            RadianceModifier.BaseMod = 0.0f;
            RadianceModifier.DamageMod = radienceTier;
            RadianceModifier.SpeedMod = radienceTier;
            RadianceModifier.HealthMod = radienceTier;
        }

        private void UnrequestBuffs()
        {
            RadianceModifier.BaseEnabled = false;
            RadianceModifier.SpeedEnabled = false;
            RadianceModifier.HealthEnabled = false;
            RadianceModifier.DamageEnabled = false;
        }

        protected void Start()
        {
            Enemy = GetComponent<EnemyComponents>();
            Eid = Enemy.Eid;
            Enemy.Radiance.AddModifier(RadianceModifier);
            RadianceModifier.SpeedEnabled = false;
            RadianceModifier.HealthEnabled = false;
            RadianceModifier.DamageEnabled = false;
            RadianceModifier.Multiplier = false;
        }

        protected void Update()
        {
            EnrageSoundCooldown -= Time.deltaTime;

            if (enrageSoundTimer > 0.0f)
            {
                enrageSoundTimer -= Time.deltaTime;

                if (enrageSoundTimer <= 0.0f)
                {
                    enrageSoundTimer = -1.0f;
                    
                    switch (Eid.enemyClass)
                    {
                        case EnemyClass.Husk:
                            TryPlayHuskEnrageSound((UnityEngine.Random.value % 0.3f) + 0.9f, Eid.bigEnemy ? 0.8f : 0.6f);
                            break;
                        case EnemyClass.Machine:
                            TryPlayMachineEnrageSound(Eid.bigEnemy ? 1.05f : 1.3f, Eid.bigEnemy ? 1.4f : 1.1f);
                            break;
                        case EnemyClass.Demon:
                            TryPlayHuskEnrageSound((UnityEngine.Random.value % 0.3f) + 0.9f, Eid.bigEnemy ? 0.85f : 0.7f);
                            break;
                        case EnemyClass.Divine:
                            break;
                        case EnemyClass.Other:
                            break;
                    }
                }
            }
        }
    }
}