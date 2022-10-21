using System.Collections.Generic;

namespace InGame.Dynamics
{
    public class Rule
    {
        /// <summary>List of ranges (areas) by rooms count</summary>
        public Dictionary<int, List<Range>> ranges = new Dictionary<int, List<Range>>();

        public void AddRange(int room, Range range)
        {
            if (ranges.ContainsKey(room) == false)
            {
                ranges.Add(room, new List<Range>());
            }
            ranges[room].Add(range);
        }
    }
}