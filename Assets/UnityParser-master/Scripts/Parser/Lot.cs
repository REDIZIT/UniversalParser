using System;

namespace UnityParser
{
    public class Lot
    {
        public Exception exception;

        [ExcelID]
        [ExcelString("Ссылка")]
        public string url;


        public Lot()
        {

        }
        public Lot(string url)
        {
            this.url = url;
        }
    }
}