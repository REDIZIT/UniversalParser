namespace InGame.Dynamics
{
    public class Range
    {
        public int? min;
        public int? max;

        public Range(string condition)
        {
            if (condition.Contains("<"))
            {
                max = int.Parse(condition.Replace("<", ""));
            }
            else if (condition.Contains(">"))
            {
                min = int.Parse(condition.Replace(">", ""));
            }
            else if (condition.Contains("-"))
            {
                string[] splitted = condition.Split('-');
                min = int.Parse(splitted[0]);
                max = int.Parse(splitted[1]);
            }
            else
            {
                throw new System.Exception($"Can't create range from string condition '{condition}'");
            }
        }

        public string GetUrlArguments()
        {
            string arg = "";

            if (min != null) arg += "&mintarea=" + min.Value;
            if (max != null) arg += "&maxtarea=" + max.Value;

            return arg;
        }
    }
}