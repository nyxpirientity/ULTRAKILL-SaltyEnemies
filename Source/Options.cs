using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace Nyxpiri.ULTRAKILL.SaltyEnemies
{
    public static class Options
    {
        public static ConfigEntry<float> DestructiveRadianceTier = null;
        public static ConfigEntry<float> ChaoticRadianceTier = null;
        public static ConfigEntry<float> BrutalRadianceTier = null;
        public static ConfigEntry<float> AnarchicRadianceTier = null;
        public static ConfigEntry<float> SupremeRadianceTier = null;
        public static ConfigEntry<float> SSadisticRadianceTier = null;
        public static ConfigEntry<float> SSSensoredStormRadianceTier = null;
        public static ConfigEntry<float> ULTRAKILLNoEnrageRadianceTier = null;
        public static ConfigEntry<float> ULTRAKILLRadianceTier = null;
        public static ConfigEntry<bool> SaltEffectHealth = null;
        public static ConfigEntry<bool> SaltEffectSpeed = null;
        public static ConfigEntry<bool> SaltEffectDamage = null;
        public static Dictionary<EnemyType, ConfigEntry<float>> Nerfs = new Dictionary<EnemyType, ConfigEntry<float>>(44);

        public static void Initialize()
        {
            DestructiveRadianceTier = Config.Bind($"Balance.Destructive", "DestructiveRadianceTier", 0.0f);
            ChaoticRadianceTier = Config.Bind($"Balance.Chaotic", "ChaoticRadianceTier", 0.0f);
            BrutalRadianceTier = Config.Bind($"Balance.Brutal", "BrutalRadianceTier", 1.0f);
            AnarchicRadianceTier = Config.Bind($"Balance.Anarchic", "AnarchicRadianceTier", 1.1f);
            SupremeRadianceTier = Config.Bind($"Balance.Supreme", "SupremeRadianceTier", 1.25f);
            SSadisticRadianceTier = Config.Bind($"Balance.SSadistic", "SSadisticRadianceTier", 1.4f);
            SSSensoredStormRadianceTier = Config.Bind($"Balance.SSSensoredStorm", "SSSensoredStormRadianceTier", 1.6f);
            ULTRAKILLNoEnrageRadianceTier = Config.Bind($"Balance.ULTRAKILL", "ULTRAKILLNoEnrageRadianceTier", 2.0f);
            ULTRAKILLRadianceTier = Config.Bind($"Balance.ULTRAKILL", "ULTRAKILLRadianceTier", 1.8f);

            SaltEffectSpeed = Config.Bind($"Balance", "SaltEffectSpeed", true);
            SaltEffectHealth = Config.Bind($"Balance", "SaltEffectHealth", false);
            SaltEffectDamage = Config.Bind($"Balance", "SaltEffectDamage", false);

            foreach (var getype in Enum.GetValues(typeof(EnemyType)))
            {
                float defaultVal = 1.0f;

                var etype = (EnemyType)getype;

                if (etype == EnemyType.Turret)
                {
                    defaultVal = 0.25f;
                }

                if (etype == EnemyType.Virtue)
                {
                    defaultVal = 0.5f;
                }

                Nerfs.Add(etype, Config.Bind($"Balance.EnemySpecific.{getype.ToString()}", "NerfScalar", defaultVal));
            }
        }
        
        internal static ConfigFile Config = null;
    }
}
