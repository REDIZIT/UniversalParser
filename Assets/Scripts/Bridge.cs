using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using Bridge;
using InGame.Dynamics;
using Newtonsoft.Json;
using RestSharp.Contrib;
using Debug = UnityEngine.Debug;

namespace Bridge
{
    public interface IArgs
    {
    }
    public class Args
    {
        public Command command;
        public object model;

        public enum Command
        {
            ScreenMaker,
            WordToPDF,
            ImageAndUrlToPDF,
        }
    }
    public class ScreenMakerArgs
    {
        public string templatePath;
        public string screenshotsPath;
        public List<ScreenMakerLot> lots;
        public string targetPath;
    }
    public class WordToPDFModel
    {
        public string docxPath, pdfPath;
    }
    public class ImageAndUrlToPDFModel
    {
        public string imagePath, url, pdfPath;
    }
}
namespace InGame
{
    public static class Bridge
    {
        public static Process Invoke(Args.Command command, object model)
        {
            ProcessStartInfo info = new(Pathes.bridgeExe);

            Args args = new()
            {
                command = command,
                model = model,
            };

            JsonSerializerSettings settings = new()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(args, settings);
            info.Arguments = '"' + HttpUtility.HtmlEncode(json) + '"';

            var proc = Process.Start(info);
            return proc;
        }
    }
}