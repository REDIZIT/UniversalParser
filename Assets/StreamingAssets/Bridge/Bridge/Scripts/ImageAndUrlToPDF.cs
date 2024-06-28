using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Bridge
{
    public class ImageAndUrlToPDFModel
    {
        public string imagePath, url, pdfPath;
    }
    public static class ImageAndUrlToPDF
    {
        public static void Handle(ImageAndUrlToPDFModel model)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var doc = Document.Create((c) =>
            {
                c.Page(page =>
                {
                    page.Content().Image(File.ReadAllBytes(model.imagePath), ImageScaling.FitArea);
                });
                c.Page(page =>
                {
                    page.Margin(24);
                    page.Content().Column(c =>
                    {
                        c.Item().Text("Дата запроса: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                        c.Item().Hyperlink(model.url).Text(model.url).FontColor(Color.FromRGB(61, 136, 204));
                    });
                });
            });

            File.WriteAllBytes(model.pdfPath, doc.GeneratePdf());
        }
    }
}