using Xceed.Words.NET;

namespace Bridge
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No .docx file path argument passed");
                Console.Read();
            }

            string filename = args[0];
            string arg2 = args[1];
            Console.Write(arg2);
            Console.Read();

            try
            {
                var doc = DocX.Load(filename);

                var sections = doc.GetSections();

                Console.WriteLine("Paragraphs count:" + doc.Paragraphs.Count);

                foreach (var paragraph in doc.Paragraphs)
                {
                    Console.WriteLine(" - " + paragraph.Text);
                }
                Console.ReadLine();

                doc.Paragraphs[0].Append(;

                doc.SaveAs(@"C:\Users\REDIZIT\Documents\GitHub\UniversalParser\Assets\StreamingAssets\Bridge\result.docx");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            

            Console.ReadLine();
        }
    }
}