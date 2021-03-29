using UnityEngine;
using UnityEditor;
using System.IO;

namespace InEditor.Preprocessors
{
    public class ScriptImportPreprocessor : UnityEditor.AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");

            if (Path.GetExtension(path) != ".cs") return;


            int index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            string file = File.ReadAllText(path);



            string lastPart = path.Substring(path.IndexOf("Assets"));
            string _namespace = lastPart.Substring(0, lastPart.LastIndexOf('/'));
            _namespace = _namespace.Replace('/', '.');
            _namespace = _namespace.Replace("Assets.Scripts.Editor", "InEditor");
            _namespace = _namespace.Replace("Assets.Scripts", "InGame");
            _namespace = _namespace.Replace("Assets.Features", "InGame.Features");
            _namespace = _namespace.Replace(".Scripts", "");

            file = file.Replace("#NAMESPACE#", _namespace);


            System.IO.File.WriteAllText(path, file);
            AssetDatabase.Refresh();
        }
    }
}