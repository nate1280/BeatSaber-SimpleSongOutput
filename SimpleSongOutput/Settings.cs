using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;

namespace SimpleSongOutput
{
    public class Settings
    {
        public bool Enabled { get; set; } = true;
        public bool SaveThumbnail { get; set; } = false;
        public string TextFilename { get; set; } = "simplesongoutput.txt";
        public string ThumbnailFilename { get; set; } = "simplesongoutput.jpg";
        public string SongTemplate { get; set; } = string.Empty;
        public string SongTemplateEmpty { get; set; } = string.Empty;
        public string SongFormat { get; set; } = "%songname%%songsubname: - % by %levelauthorname% on %difficulty%";
        public int ThumbnailSize { get; set; } = 256;

        [JsonIgnore]
        private static readonly string Filename = Path.Combine(Plugin.DataPath, "SimpleSongOutput.json");

        [JsonIgnore]
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), Formatting = Formatting.Indented };

        public void Set(string name, object value)
        {
            object obj = this;
            var props = obj.GetType().GetProperties();

            // check if the property exists
            if (!props.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))) return;

            // get the property to update
            var prop = props.First(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

            // set the property
            prop.SetValue(this, value);

            // save
            Save();
        }

        public static Settings Load()
        {
            var _settings = new Settings();
            if (File.Exists(Filename))
            {
                var data = File.ReadAllText(Filename);
                _settings = JsonConvert.DeserializeObject<Settings>(data);
            }
            _settings.Save();
            return _settings;
        }

        public void Save()
        {
            var data = JsonConvert.SerializeObject(this, serializerSettings);
            File.WriteAllText(Filename, data);
        }
    }
}
