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
            if (args.Length < 1)
            {
                Console.WriteLine("No .docx file path argument passed");
                Console.Read();
            }

            ScreenMakerLot lot = JsonConvert.DeserializeObject<ScreenMakerLot>(HttpUtility.HtmlDecode(args[1]));

            string filename = args[0];

            try
            {
                doc = DocX.Load(filename);

                string price = lot.price.Split(',')[0];
                string priceAbilities = string.Join(',', lot.price.Split(',').Skip(1));

                string[] areas = lot.area.Split('/');

                float _price = float.Parse(price.Split()[0]);
                float _area = float.Parse(areas[0].Replace('.', ','));
                float pricePerMeter = _price / _area;

                string levels = lot.levels.Split(",")[0];
                string type = string.Join(',', lot.levels.Split(',').Skip(1)).ToLower().Trim();

                Replace("price", _price.ToString());
                Replace("price_abilities", priceAbilities);
                Replace("price_per_meter", pricePerMeter.ToString());
                Replace("address", lot.address);
                Replace("metro", lot.metro);
                Replace("levels", levels);
                Replace("type", type);
                Replace("description", lot.description);
                Replace("date", DateTime.Parse(lot.date).ToString("M") + ", " + DateTime.Parse(lot.date).ToString("t"));

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

                doc.SaveAs(@"C:\Users\REDIZIT\Documents\GitHub\UniversalParser\Assets\StreamingAssets\Bridge\result.docx");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }

            Console.WriteLine("Done");
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