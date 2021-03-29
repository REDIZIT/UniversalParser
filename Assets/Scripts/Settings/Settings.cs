using Newtonsoft.Json;

namespace InGame.Settings
{
	public class Settings
	{
        /// <summary>Path to folder, where user last saved or loaded file</summary>
        [JsonIgnore]
        public string FilebrowserLastUsedDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(filebrowserLastUsedDirectory))
                {
                    return Pathes.buildFolder;
                }
                return filebrowserLastUsedDirectory;
            }
            set
            {
                filebrowserLastUsedDirectory = value;
            }
        }

        [JsonProperty]
        private string filebrowserLastUsedDirectory;
    }
}