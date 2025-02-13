using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InGame.Recognition
{
    public static class RecognizerStoreys
    {
        private static string pathToAreaPatternsFile = "Recognition/storeys_patterns";

        private static string[] areaPatterns;


        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            areaPatterns = Resources.Load<TextAsset>(pathToAreaPatternsFile).text.Replace("\n", "").Split('\r');
        }



        public static bool TryExtractStoreysString(string text, out string area)
        {
            bool way1 = TryCommaSeparatedWay(text, out string way1str);
            //bool way2 = TrySpaceSeparatedWay(text, out string way2str);

            // Give prioriy for space separated way
            //area = way2 ? way2str : way1 ? way1str : null;
            area = Recognizer.TrimCommas(way1str);
            return way1 /*|| way2*/;
        }
        public static bool IsStoreysString(string text)
        {
            return TryExtractStoreysString(text, out _);
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
                    // For example: � �. ������������, ���������, 20 ���
                    // Letter � isn't a marker for area, because this is shortcut of ����� (metro)
                    // 20 ��� is area

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
                    bool hasDigitOnLeft = ContainsDigitBeforeIndex(patternStart, str, 2);


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

                string pattern = TryGetSpacePattern(splitted, i);
                if (pattern == null) continue;

                int patternStart = str.IndexOf(pattern);

                // If pattern could not be found in str (splitted part of fullstring)
                // that means, that in TryGetSpacePattern used next str (i + 1)
                // Unite this str with next str and find indexes again
                if (patternStart == -1)
                {
                    str += " " + splitted[i + 1];
                    patternStart = str.IndexOf(pattern);
                }

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
                    string notContinuesArea = str;
                    foreach (string prevStr in EnumerateArrayBackwards(splitted, i - 1))
                    {
                        if (prevStr.Any(c => char.IsDigit(c)))
                        {
                            notContinuesArea = prevStr + " " + notContinuesArea;
                        }
                    }
                    if (notContinuesArea != str)
                    {
                        area = notContinuesArea;
                        return true;
                    }

                }
            }


            area = null;
            return false;
        }
        private static string TryGetSpacePattern(string[] splitted, int i)
        {
            if (i < splitted.Length - 1 && splitted.Length > 1)
            {
                // Try to untite this splitted string with next one
                // If we will found pattern with this untied string return that
                string str1 = splitted[i];
                string nextStr = splitted[i + 1];
                string united = str1 + " " + nextStr;

                string pattern1 = areaPatterns.FirstOrDefault(p => united.Contains(p));
                if (string.IsNullOrEmpty(pattern1) == false) return pattern1;
            }

            // Not supporting patterns with space
            string str = splitted[i];
            string pattern = areaPatterns.FirstOrDefault(p => str.Contains(p));
            return pattern;
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
        private static IEnumerable<string> EnumerateArrayBackwards(string[] array, int startIndex)
        {
            for (int i = startIndex; i >= 0; i--)
            {
                yield return array[i];
            }
        }
    }
}