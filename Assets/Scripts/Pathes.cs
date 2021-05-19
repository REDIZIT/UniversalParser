using System;
using System.IO;
using UnityEngine;

namespace InGame
{
    public static class Pathes
	{
        public static string buildFolder;
        public static string settingsFile;
        public static string dataFolder;

        public static void Initialize()
        {
            dataFolder = Application.dataPath;
            buildFolder = new DirectoryInfo(dataFolder).Parent.FullName;

            settingsFile = buildFolder + "/settings.json";
        }
	}
}