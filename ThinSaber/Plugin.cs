using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;
using IPA;
using IPA.Config.Stores;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;


namespace ThinSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static string PluginName => "ThinSaber";
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        public static bool IsBSUtilsAvailable => true;
        public static Boolean TweakEnabled => Config.Instance.Enabled;

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            Config.Instance = conf.Generated<Config>();
            Log.Info("ThinSaber initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnEnable]
        public void OnEnable()
        {
            BSMLSettings.instance.AddSettingsMenu("ThinSaber", "ThinSaber.Views.Views.bsml", Config.Instance);
            GameplaySetup.instance.AddTab("ThinSaber", "ThinSaber.Views.Views.bsml", Config.Instance, MenuType.All);
        }

        [OnDisable]
        public void OnDisable()
        {
            BSMLSettings.instance.RemoveSettingsMenu(Config.Instance);
            GameplaySetup.instance.RemoveTab("ThinSaber");
        }

        [OnStart]
        public void Load()
        {
            AddEvents();
            Log.Debug("OnApplicationStart");
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "GameCore")
            {
                if (TweakEnabled)
                {
                    new GameObject(PluginName).AddComponent<SaberLength>();
                }
            }
        }

        private void AddEvents()
        {
            RemoveEvents();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void RemoveEvents()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        [OnExit]
        public void Unload()
        {
            RemoveEvents();
            Log.Debug("OnApplicationQuit");

        }
    }
}
