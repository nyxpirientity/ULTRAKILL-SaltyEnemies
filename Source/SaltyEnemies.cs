using UnityEngine;
using BepInEx;
using Nyxpiri.ULTRAKILL.NyxLib;
using System.IO;

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
            Options.Config = Config;
            Options.Initialize();
            
            if (!File.Exists(Config.ConfigFilePath))
            {
                Config.Save();
            }
        }

        protected void Start()
        {
        }

        protected void Update()
        {

        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Config.Reload();
            }
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
