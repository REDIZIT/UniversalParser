using System;

namespace UnityParser
{
    public class Lot
    {
        public Exception exception;

        [ExcelID]
        [ExcelString("—сылка")]
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