using System;
using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Linq;

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

    public static class Program
    {
        public static void Main(string[] arg)
        {
            if (arg.Length == 0)
            {
                PrintHeader();
                Console.WriteLine("- No argument passed");
            }
            else
            {
                Args args = null;

                try
                {
                    string json = HttpUtility.HtmlDecode(arg[0]);
                    args = JsonConvert.DeserializeObject<Args>(json);
                }
                catch (Exception err)
                {
                    Console.WriteLine("- Exception while argument json deserialization:");
                    Console.WriteLine(err);
                    Console.WriteLine("");
                    Console.WriteLine("- Argument json:");
                    string json = HttpUtility.HtmlDecode(arg[0]);
                    Console.WriteLine(json);
                }

                if (args != null)
                {
                    try
                    {
                        JObject model = (JObject) args.model;
                        if (args.command == Args.Command.ScreenMaker)
                        {
                            ScreenMaker.Handle(model.ToObject<ScreenMakerArgs>());
                        }
                        else if (args.command == Args.Command.WordToPDF)
                        {
                            WordToPDF.Handle(model.ToObject<WordToPDFModel>());
                        }
                        else if (args.command == Args.Command.ImageAndUrlToPDF)
                        {
                            ImageAndUrlToPDF.Handle(model.ToObject<ImageAndUrlToPDFModel>());
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Command execution block exception:");
                        Console.WriteLine(err.ToString());

                        Console.ReadLine();
                    }
                }

            }


            Console.WriteLine();
            Console.WriteLine("Program end, press any to exit");
        }

        private static void PrintHeader()
        {
            Console.WriteLine("[ Universal Parser Bridge ]");
            Console.WriteLine("");
            Console.WriteLine("This console app has been created only for one purpose - use C# not in Unity bounds, but in native windows space");
            Console.WriteLine("To use this app Unity-side should start Bridge.exe with json serialized (with type serialization) argument.");
            Console.WriteLine("");
            Console.WriteLine("Failed to define instructions");
        }
    }
}