using System.IO;
using System.Threading;
using InGame.Dynamics;
using UnityEngine;

namespace InGame
{
    public static class Pathes
	{
        public static string buildFolder;
        public static string settingsFile;
        public static string dataFolder;
        public static string steamingAssets;
        public static string bridgeExe;

        public static void Initialize()
        {
            dataFolder = Application.dataPath;
            buildFolder = new DirectoryInfo(dataFolder).Parent.FullName;

            settingsFile = buildFolder + "/settings.json";
            steamingAssets = dataFolder + "/StreamingAssets";

            bridgeExe = steamingAssets + "/Bridge/Bridge/bin/Debug/net8.0/Bridge.exe";
        }
	}
}