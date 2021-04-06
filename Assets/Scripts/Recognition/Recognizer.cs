namespace InGame.Recognition
{
    public static class Recognizer
    {
        public class Result
        {
            public string name;
            public string area;
            public string storeys;
        }


        /// <summary>Tries to split human written text to object name, area and storeys</summary>
        public static bool TryRecognize(string text, out Result result)
        {
            result = new Result();

            bool areaSuccess = RecognizerArea.TryExtractAreaString(text, out result.area);
            bool storeysSuccess = RecognizerStoreys.TryExtractStoreysString(text, out result.storeys);

            string name = text;
            if (string.IsNullOrEmpty(result.area) == false) name = name.Replace(result.area, "");
            if (string.IsNullOrEmpty(result.storeys) == false) name = name.Replace(result.storeys, "");

            result.name = TrimCommas(name);

            return areaSuccess || storeysSuccess;
        }

        public static string TrimCommas(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return text.Trim(new char[] { ' ', ',' });
        }
    }
}