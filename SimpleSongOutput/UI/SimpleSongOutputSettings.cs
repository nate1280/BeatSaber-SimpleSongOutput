using BeatSaberMarkupLanguage.Attributes;

namespace SimpleSongOutput.UI
{
    public class SimpleSongOutputSettings : PersistentSingleton<SimpleSongOutputSettings>
    {
        [UIValue("enabled")]
        public bool Enabled
        {
            get => Plugin.cfg.Enabled;
            set => Plugin.cfg.Set("Enabled", value);
        }

        [UIValue("output-thumbnail")]
        public bool SaveThumbnail
        {
            get => Plugin.cfg.SaveThumbnail;
            set => Plugin.cfg.Set("SaveThumbnail", value);
        }
    }
}
