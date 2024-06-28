namespace Bridge
{
    public class WordToPDFModel : IArgs
    {
        public string docxPath, pdfPath;
    }
    public static class WordToPDF
    {
        public static void Handle(WordToPDFModel model)
        {
            //
            // Attention!
            // Spire will make watermarks about evaluation expiration licence
            //

            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(model.docxPath);
            document.SaveToFile(model.pdfPath);
            document.Dispose();
        }
    }
}