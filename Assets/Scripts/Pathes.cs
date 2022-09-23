using System.IO;
using UnityEngine;

namespace InGame
{
    public static class Pathes
	{
        public static string buildFolder;
        public static string settingsFile;
        public static string dataFolder;
        public static string steamingAssets;

        public static void Initialize()
        {
            dataFolder = Application.dataPath;
            buildFolder = new DirectoryInfo(dataFolder).Parent.FullName;

            settingsFile = buildFolder + "/settings.json";
            steamingAssets = dataFolder + "/StreamingAssets";
        }
	}
}