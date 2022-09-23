using InGame;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;

namespace InEditor
{
    public class BuildPostprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            return;

            string[] splitted = PlayerSettings.bundleVersion.Split('.');

            string gitCommitNumber = GetVersion();
            splitted[splitted.Length - 1] = gitCommitNumber;

            PlayerSettings.bundleVersion = string.Join(".", splitted);
        }

        private string GetVersion()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + "git rev-list --count HEAD");


            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd().Replace("\n", "").Replace("\r", "");
            return result;
        }
    }
}