using System.Collections.Generic;

namespace InGame.Dynamics
{
    public class UrlsCreator
    {
        private List<string> districts;
        private Dictionary<string, Rule> rules;

        private string[] roomsArgs = new string[5]
        {
            "&room1=1",
            "&room2=1",
            "&room3=1",
            "&room4=1&room5=1&room6=1",
            "&room7=1&room9=1"
        };

        public List<string> Create()
        {
            Excel sourceTable = ExcelHelper.LoadExcel(Pathes.steamingAssets + "/source_table.xlsx");
            ExtractDistricts(sourceTable);
            ExtractRules(sourceTable);

            return CreateUrls();
        }

        private void ExtractDistricts(Excel table)
        {
            ExcelTable districtTable = table.Tables[0];

            districts = new List<string>();
            for (int i = 1; i <= districtTable.NumberOfRows; i++)
            {
                var cell = districtTable.GetCell(i + 1, 1);
                if (cell == null) continue;

                districts.Add(cell.Value);
            }
        }
        private void ExtractRules(Excel table)
        {
            ExcelTable rulesTable = table.Tables[1];

            rules = new Dictionary<string, Rule>();


            int currentRoom = 0;
            for (int i = 8; i <= rulesTable.NumberOfRows; i++)
            {
                // Update room index
                var roomCell = rulesTable.GetCell(i, 1);
                if (roomCell != null && string.IsNullOrWhiteSpace(roomCell.Value) == false && roomCell.Value.Contains("https") == false)
                {
                    currentRoom++;
                }

                // Check district
                var districtCell = rulesTable.GetCell(i, 2);
                if (districtCell == null) continue;

                string district = districtCell.Value;
                Rule rule = GetOrCreateRule(district);


                // Add ranges
                for (int x = 0; x < 5; x++)
                {
                    var conditionCell = rulesTable.GetCell(i, 4 + x);
                    if (conditionCell == null || string.IsNullOrWhiteSpace(conditionCell.Value)) continue;

                    rule.AddRange(currentRoom - 1, new Range(conditionCell.Value));
                }
            }
        }
        private List<string> CreateUrls()
        {
            List<string> urls = new List<string>();



            foreach (string district in districts)
            {
                if (rules.TryGetValue(district, out Rule rule))
                {
                    for (int r = 0; r < roomsArgs.Length; r++)
                    {
                        if (rule.ranges.TryGetValue(r, out List<Range> ranges))
                        {
                            foreach (Range range in ranges)
                            {
                                urls.Add(CreateUrl(district, r, range));
                            }
                        }
                        else
                        {
                            urls.Add(CreateUrl(district, r, null));
                        }
                    }
                }
                else
                {
                    for (int r = 0; r < roomsArgs.Length; r++)
                    {
                        urls.Add(CreateUrl(district, r, null));
                    }
                }
            }

            return urls;
        }
        private string CreateUrl(string district, int rooms, Range range)
        {
            string url = $"https://spb.cian.ru/cat.php?deal_type=sale&district%5B0%5D={district}&engine_version=2&object_type%5B0%5D=1&offer_type=flat&totime=864000";

            url += roomsArgs[rooms];

            if (range != null)
            {
                url += range.GetUrlArguments();
            }

            return url;
        }

        private Rule GetOrCreateRule(string district)
        {
            if (rules.ContainsKey(district) == false)
            {
                rules.Add(district, new Rule());
            }
            return rules[district];
        }
    }
}