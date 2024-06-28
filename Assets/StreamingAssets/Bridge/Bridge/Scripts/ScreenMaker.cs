using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InGame.Dynamics;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Bridge
{
    public class ScreenMakerArgs : IArgs
    {
        public string templatePath;
        public string screenshotsPath;
        public List<ScreenMakerLot> lots;
        public string targetPath;
    }
    public static class ScreenMaker
    {
        private static DocX doc;

        private const string AREA_METER = " м²";
        private const string GAP = "—";

        public static void Handle(ScreenMakerArgs screenMakerArgs)
        {
            foreach (var lot in screenMakerArgs.lots)
            {
                string savePath = screenMakerArgs.targetPath + lot.id;

                Console.WriteLine("savePath = " + savePath);

                string filename = screenMakerArgs.templatePath;

                Console.WriteLine("filename = " + filename);

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

                    Random rnd = new(int.Parse(lot.id));

                    DateTime dateTime = DateTime.Parse(lot.date);
                    dateTime = dateTime.Add(new TimeSpan(rnd.Next(6, 23), rnd.Next(0, 59), rnd.Next(0, 59)));

                    Replace("price", _price.ToString());
                    Replace("price_abilities", priceAbilities);
                    Replace("price_per_meter", pricePerMeter.ToString());
                    Replace("address", lot.address);
                    Replace("metro", lot.metro);
                    Replace("levels", levels);
                    Replace("type", type);
                    Replace("description", lot.description.Replace("\n", "").Trim());
                    Replace("date", dateTime.ToString("M") + ", " + dateTime.ToString("t"));

                    Replace("lift", lot.lift);
                    Replace("balcony", lot.balcony);
                    Replace("bathroom", lot.bathroom);
                    Replace("view", lot.outsideView);

                    Replace("area", areas.Length >= 1 ? areas[0] + AREA_METER : "", areas.Length < 1);
                    Replace("area_hab", areas.Length >= 2 ? areas[1] + AREA_METER : "", areas.Length < 2);
                    Replace("area_kitchen", areas.Length >= 3 ? areas[2] + AREA_METER : "", areas.Length < 3);

                    Replace("area_rooms", lot.areaRooms + AREA_METER, string.IsNullOrWhiteSpace(lot.areaRooms));
                    Replace("hasTel", string.IsNullOrWhiteSpace(lot.hasTel) ? GAP : "нет");

                    Replace("rooms", lot.rooms + " кв.");
                    Replace("id", lot.id);
                    Replace("phones", lot.phones);
                    Replace("footer_year", dateTime.Year.ToString());


                    // Replace image

                    if (string.IsNullOrWhiteSpace(lot.screenshot) == false)
                    {
                        Console.WriteLine("screenshot: " + lot.screenshot);

                        var names = Directory.GetFiles(screenMakerArgs.screenshotsPath).Where(p => Path.GetFileName(p).StartsWith(lot.screenshot) && Path.GetExtension(p) != ".meta");
                        if (names.Count() > 0)
                        {
                            Console.WriteLine("Names: " + string.Join("\n", names));
                            ReplaceImage(names.ElementAt(rnd.Next(0, names.Count() - 1)));
                        }
                    }

                    // Save .docx

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
                    document.Dispose();
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
        private static void ReplaceImage(string imagePath)
        {
            var img = doc.AddImage(imagePath);

            doc.Paragraphs[38].ReplacePicture(doc.Paragraphs[38].Pictures[0], img.CreatePicture());
        }
    }
}