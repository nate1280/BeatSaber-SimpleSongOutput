using System;
using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using System.IO;
using Harmony;
using BS_Utils.Utilities;
using BeatSaberMarkupLanguage.Settings;
using SimpleSongOutput.UI;

namespace SimpleSongOutput
{
    public class Plugin : IBeatSaberPlugin
    {
        public static SemVer.Version Version => IPA.Loader.PluginManager.GetPlugin("SimpleSongOutput").Metadata.Version;

        public static IPALogger Log { get; internal set; }

        public static Settings cfg;
        public static string DataPath = Path.Combine(Environment.CurrentDirectory, "UserData", "SimpleSongOutput");

        public static string FullTextFilename => Path.Combine(DataPath, Plugin.cfg.TextFilename);
        public static string FullThumbnailFilename => Path.Combine(DataPath, Plugin.cfg.ThumbnailFilename);

        public void Init(object thisIsNull, IPALogger log)
        {
            Log = log;
        }

        public void OnApplicationStart()
        {
            // create userdata path if needed
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            // load settings
            cfg = Settings.Load();

            BSEvents.OnLoad();
            BSEvents.menuSceneLoadedFresh += OnMenuSceneLoadedFresh;
        }

        private void OnMenuSceneLoadedFresh()
        {
            // add BSML mod settings
            BSMLSettings.instance.AddSettingsMenu("Simple Song Output", "SimpleSongOutput.Views.SimpleSongOutputSettings.bsml", SimpleSongOutputSettings.instance);

            // load main mod
            SimpleSongOutput.Instance.OnLoad();
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

        public void OnSceneLoaded(Scene scene, LoadSceneMode arg1) { }

        public void OnSceneUnloaded(Scene scene) { }

        public void OnApplicationQuit() { }

        public void OnLevelWasLoaded(int level) { }

        public void OnLevelWasInitialized(int level) { }

        public void OnUpdate() { }

        public void OnFixedUpdate() { }
    }
}
