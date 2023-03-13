using InGame.Dynamics;
using Newtonsoft.Json;
using System.Web;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Bridge
{
    public static class Program
    {
        private static DocX doc;

        private const string AREA_METER = " м²";
        private const string GAP = "—";

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Bridge launched. Arguments count: " + args.Length);

                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("\n=== Argument " + i + "===");
                    Console.WriteLine(args[i]);
                    Console.WriteLine();
                }

                Console.WriteLine("Press any key to continue");
                Console.Read();

                if (args.Length < 1)
                {
                    Console.WriteLine("No .docx file path argument passed");
                    Console.Read();
                }

                ScreenMakerLot lot = JsonConvert.DeserializeObject<ScreenMakerLot>(HttpUtility.HtmlDecode(args[1]));

                Console.WriteLine("Lot created = " + lot);
                Console.WriteLine("Press any key to continue");
                Console.Read();

                string savePath = HttpUtility.HtmlDecode(args[2]);

                Console.WriteLine("savePath = " + savePath);
                Console.WriteLine("Press any key to continue");
                Console.Read();

                string filename = HttpUtility.HtmlDecode(args[0]);

                Console.WriteLine("filename = " + filename);
                Console.WriteLine("Press any key to continue");
                Console.Read();

                try
                {
                    Console.WriteLine("Template block enter");

                    doc = DocX.Load(filename);

                    Console.WriteLine("Tempate loaded");

                    string price = lot.price.Split(',')[0];
                    string priceAbilities = string.Join(',', lot.price.Split(',').Skip(1));

                    string[] areas = lot.area.Split('/');

                    float _price = float.Parse(price.Split()[0]);
                    float _area = float.Parse(areas[0].Replace('.', ','));
                    float pricePerMeter = _price / _area;

                    string levels = lot.levels.Split(",")[0];
                    string type = string.Join(',', lot.levels.Split(',').Skip(1)).ToLower().Trim();

                    DateTime dateTime = DateTime.Parse(lot.date);

                    Replace("price", _price.ToString());
                    Replace("price_abilities", priceAbilities);
                    Replace("price_per_meter", pricePerMeter.ToString());
                    Replace("address", lot.address);
                    Replace("metro", lot.metro);
                    Replace("levels", levels);
                    Replace("type", type);
                    Replace("description", lot.description);
                    Replace("date", dateTime.ToString("M") + ", " + dateTime.ToString("t"));

                    Replace("lift", lot.lift);
                    Replace("balcony", lot.balcony);
                    Replace("bathroom", lot.bathroom);
                    Replace("view", lot.outsideView);
                    Replace("area", areas[0] + AREA_METER, string.IsNullOrWhiteSpace(areas[0]));
                    Replace("area_hab", areas[1] + AREA_METER, string.IsNullOrWhiteSpace(areas[1]));
                    Replace("area_kitchen", areas[2] + AREA_METER, string.IsNullOrWhiteSpace(areas[2]));
                    Replace("area_rooms", lot.areaRooms + AREA_METER, string.IsNullOrWhiteSpace(lot.areaRooms));
                    Replace("hasTel", string.IsNullOrWhiteSpace(lot.hasTel) ? GAP : "нет");

                    Replace("rooms", lot.rooms);
                    Replace("phones", lot.phones);

                    Console.WriteLine("Tempate replaced");

                    string docx = savePath + ".docx";
                    string pdf = savePath + ".pdf";

                    Console.WriteLine("docx = " + docx);
                    Console.WriteLine("pdf = " + pdf);

                    doc.SaveAs(docx);
                    Console.WriteLine("Docx saved");

                    // Converting to .pdf
                    Spire.Doc.Document document = new Spire.Doc.Document();
                    document.LoadFromFile(docx);
                    Console.WriteLine("Docx loaded");
                    document.SaveToFile(pdf);
                    Console.WriteLine("Pdf saved");

                    File.Delete(docx);

                    Console.WriteLine("Docx deleted");

                    File.SetCreationTime(pdf, dateTime);
                    File.SetLastWriteTime(pdf, dateTime);
                    File.SetLastAccessTime(pdf, dateTime);

                    Console.WriteLine("File dates changed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ReadLine();
                }

                Console.WriteLine("Done");
            }
            catch (Exception err)
            {
                Console.WriteLine("Main block exception:");
                Console.WriteLine(err.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Program end, press any to exit");
            Console.ReadLine();
        }
        private static void Replace(string name, string newValue, bool? isEmpty = null)
        {
            bool isWhiteSpace = isEmpty.HasValue ? isEmpty.Value : string.IsNullOrWhiteSpace(newValue);

            doc.ReplaceText(new StringReplaceTextOptions()
            {
                SearchValue = "{" + name + "}",
                NewValue = isWhiteSpace ? GAP : newValue
            });
        }
    }
}