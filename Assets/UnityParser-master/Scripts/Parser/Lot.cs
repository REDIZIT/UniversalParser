using System;

namespace UnityParser
{
    public class Lot
    {
        public Exception exception;

        [ExcelID]
        [ExcelString("Ссылка")]
        public string url;
    }
}