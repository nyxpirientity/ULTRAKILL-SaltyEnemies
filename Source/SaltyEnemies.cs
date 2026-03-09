using UnityEngine;
using BepInEx;
using Nyxpiri.ULTRAKILL.NyxLib;

namespace Nyxpiri.ULTRAKILL.SaltyEnemies
{
    public static class Cheats
    {
        public const string SaltyEnemies = "nyxpiri.salty-enemies";
    }

    [BepInPlugin("com.nyxpiri.bepinex.plugins.ultrakill.salty-enemies", "Salty Enemies", "0.0.0.1")]
    [BepInProcess("ULTRAKILL.exe")]
    public class SaltyEnemies: BaseUnityPlugin
    {
        protected void Awake()
        {
            Log.Initialize(Logger);
            SaltyEnemy.Initialize();
            NyxLib.Cheats.ReadyForCheatRegistration += RegisterCheats;
        }

        protected void Start()
        {
        }

        protected void Update()
        {

        }

        protected void LateUpdate()
        {

        }

        private void RegisterCheats(CheatsManager cheatsManager)
        {
            cheatsManager.RegisterCheat(new ToggleCheat(
                "Salty Enemies", 
                Cheats.SaltyEnemies,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "ENEMY'S MIND");
        }
    }
}
