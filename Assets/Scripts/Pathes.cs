using System;
using System.IO;
using UnityEngine;

namespace InGame
{
    public static class Pathes
	{
        public static string buildFolder;
        public static string settingsFile;

        public static void Initialize()
        {
            buildFolder = new DirectoryInfo(Application.dataPath).Parent.FullName;

            settingsFile = buildFolder + "/settings.json";
        }
	}
}