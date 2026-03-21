using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.SaltyEnemies
{
    public static class Assets
    {
        public static GameObject HuskEnrageSound_0 { get; private set; } = null;
        public static GameObject MachineEnrageSound_0 { get; private set; } = null;
        public static AudioClip MachineEnrageSound_1 { get; private set; } = null;
        
        internal static void Initialize()
        {
            NyxLib.Assets.AddAssetPicker<SwordsMachine>((sm) =>
            {
                if (sm.bigPainSound == null)
                {
                    return false;
                }

                MachineEnrageSound_0 = UnityEngine.Object.Instantiate(sm.bigPainSound, null, false);
                
                if (MachineEnrageSound_0 == null)
                {
                    return false;
                }
                

                MachineEnrageSound_0.SetActive(false);

                UnityEngine.Object.DontDestroyOnLoad(MachineEnrageSound_0);

                return true;
            });

            NyxLib.Assets.AddAssetPicker<Streetcleaner>((sc) =>
            {
                var enemy = sc.GetComponent<Enemy>();
                
                if (enemy == null || enemy.deathSound == null)
                {
                    return false;
                }

                MachineEnrageSound_1 = UnityEngine.Object.Instantiate(enemy.deathSound, null, false);
                
                if (MachineEnrageSound_1 == null)
                {
                    return false;
                }

                UnityEngine.Object.DontDestroyOnLoad(MachineEnrageSound_1);

                return true;
            });

            NyxLib.Assets.AddAssetPicker<StatueBoss>((sb) =>
            {
                if (sb.statueChargeSound2 == null)
                {
                    return false;
                }

                HuskEnrageSound_0 = GameObject.Instantiate(sb.statueChargeSound2);
                HuskEnrageSound_0.SetActive(false);
                
                if (HuskEnrageSound_0 == null)
                {
                    return false;
                }

                var removeOnTime = HuskEnrageSound_0.GetOrAddComponent<RemoveOnTime>();
                removeOnTime.time = 1.0f;

                UnityEngine.Object.DontDestroyOnLoad(HuskEnrageSound_0);

                return true;
            });

        }
    }
}