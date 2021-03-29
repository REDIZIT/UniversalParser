using Newtonsoft.Json;
using System.IO;

namespace InGame.Settings
{
	public static class SettingsManager
	{
		public static Settings settings
        {
            get
            {
                if (_settings == null) Load();
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        private static Settings _settings;

        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };



        public static void Load()
        {
            if (File.Exists(Pathes.settingsFile) == false)
            {
                _settings = new Settings();
                Save();
                return;
            }

            string json = File.ReadAllText(Pathes.settingsFile);
            _settings = JsonConvert.DeserializeObject<Settings>(json, jsonSettings);
            return;
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(_settings, jsonSettings);
            File.WriteAllText(Pathes.settingsFile, json);
        }
	}
}