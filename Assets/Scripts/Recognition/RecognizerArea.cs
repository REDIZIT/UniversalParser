using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InGame.Recognition
{
	public static class RecognizerArea 
	{
        private static string pathToAreaPatternsFile = "Recognition/area_patterns";

        private static string[] areaPatterns;


        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            areaPatterns = Resources.Load<TextAsset>(pathToAreaPatternsFile).text.Replace("\n", "").Split('\r');
        }


        public static string TryExtractAreaString(string fullstring)
        {
            bool way1 = TryCommaSeparatedWay(fullstring, out string way1str);
            bool way2 = TrySpaceSeparatedWay(fullstring, out string way2str);

            //Debug.Log($"For string '{fullstring}' comma is {(way1 ? way1str : "-")} and space is {(way2 ? way2str : "-")}");

            // Give prioriy for space separated way
            return way2 ? way2str : way1str;
        }

        private static bool TryCommaSeparatedWay(string fullstring, out string area)
        {
            area = null;

            // Go simple and more common way
            // Split text by comma and try to find area in spliited strings
            IEnumerable<string> splitted = Split(fullstring);

            foreach (string str in splitted)
            {
                // If pattern detected in splitted string
                string pattern = areaPatterns.FirstOrDefault(p => str.Contains(p));

                if (string.IsNullOrEmpty(pattern) == false)
                {
                    // Check if pattern isn't contained in word
                    // For example: У м. Чернышевская, проходное, 20 квт
                    // Letter м isn't a marker for area, because this is shortcut of метро (metro)
                    // 20 квт is area

                    int patternStart = str.IndexOf(pattern);
                    int patternEnd = patternStart + pattern.Length;

                    // Check is any letter on side of marker in str
                    bool isLetterOnLeft = false;
                    bool isLetterOnRight = false;

                    if (patternStart > 1)
                    {
                        isLetterOnLeft = char.IsLetter(str[patternStart - 1]);
                    }
                    if (patternEnd < str.Length - 1)
                    {
                        isLetterOnRight = char.IsLetter(str[patternEnd + 1]);
                    }


                    // Check digits on left of marker
                    // Does str contain any digit on left two chars. When area - true, when word part - false
                    bool hasDigitOnLeft = false;

                    for (int i = 1; i <= 2; i++)
                    {
                        int index = patternStart - i;

                        hasDigitOnLeft = char.IsDigit(str[index]);

                        if (hasDigitOnLeft) break;
                    }



                    if (isLetterOnLeft == false && isLetterOnRight == false && hasDigitOnLeft)
                    {
                        area = str;
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool TrySpaceSeparatedWay(string fullstring, out string area)
        {
            string[] splitted = fullstring.Split(' ');

            for (int i = 0; i < splitted.Length; i++)
            {
                string str = splitted[i];


                string pattern = areaPatterns.FirstOrDefault(p => str.Contains(p));
                if (pattern == null) continue;

                int patternStart = str.IndexOf(pattern);
                int patternEnd = patternStart + pattern.Length;


                // Lets take a first variant of presenting area 260m^2 - continuous spelling
                bool isContinuousSpelling = ContainsDigitBeforeIndex(patternStart, str, int.MaxValue);

                if (isContinuousSpelling)
                {
                    area = str;
                    return true;
                }

                // If not continuous spelling, we need to check previous splitted string for a number
                if (i != 0)
                {
                    string prevStr = splitted[i - 1];
                    if (prevStr.Any(c => char.IsDigit(c)))
                    {
                        area = prevStr + " " + str;
                        return true;
                    }
                }
            }


            area = null;
            return false;
        }

     
        private static IEnumerable<string> Split(string str)
        {
            // Replace comma in area marker with spec symbols to ignore these on str.Split(',') step
            // 17.1 is okay
            // 17,1 will be splitted into 2 string, this method fixes that


            MatchEvaluator evaluator = new MatchEvaluator((m) =>
            {
                return m.Value.Replace(",", "/*.*/");
            });



            str = Regex.Replace(str, @"\d,\d", evaluator);

            foreach (string s in str.Split(','))
            {
                yield return s.Replace("/*.*/", ",").Trim();
            }
        }

        private static bool ContainsDigitBeforeIndex(int patternStart, string str, int maxChars)
        {
            for (int i = 1; i <= maxChars; i++)
            {
                int index = patternStart - i;
                if (index < 0 || index >= str.Length) return false;
                if (char.IsDigit(str[index])) return true;
            }
            return true;
        }
    }
}