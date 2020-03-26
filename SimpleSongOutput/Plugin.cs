using System;
using IPA;
using Logger = IPA.Logging.Logger;
using System.IO;
using BS_Utils.Utilities;
using BeatSaberMarkupLanguage.Settings;
using SimpleSongOutput.UI;

namespace SimpleSongOutput
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static SemVer.Version Version => IPA.Loader.PluginManager.GetPlugin("SimpleSongOutput").Version;

        public static Logger Log { get; internal set; }

        public static Settings cfg;
        public static string DataPath = Path.Combine(Environment.CurrentDirectory, "UserData", "SimpleSongOutput");

        public static string FullTextFilename => Path.Combine(DataPath, Plugin.cfg.TextFilename);
        public static string FullThumbnailFilename => Path.Combine(DataPath, Plugin.cfg.ThumbnailFilename);

        [Init]
        public void Init(Logger log)
        {
            Log = log;
        }

        [OnStart]
        public void OnStart()
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
    }
}
