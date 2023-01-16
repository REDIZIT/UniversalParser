using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dynamics
{
    public class UrlsCreator
    {
        private List<string> districts;
        private Dictionary<string, Rule> rules;
        private Rule specialRule;
        private string baseUrl;

        private string[] roomsArgs = new string[5]
        {
            "&room1=1",
            "&room2=1",
            "&room3=1",
            "&room4=1&room5=1&room6=1",
            "&room7=1&room9=1"
        };

        public enum Type
        {
            /// <summary>Вторичка (районы + площади)</summary>
            SaleDistrictArea,
            /// <summary>Аренда (районы + площади)</summary>
            RentDistrictArea,
            /// <summary>Комуналки (только районы)</summary>
            SaleRoomsDistrict,
            /// <summary>Апартаменты по новым правилам  (только площади)</summary>
            SaleApartmentsSpecial,
            /// <summary>Первичка по новым правилам  (только площади)</summary>
            SaleFirstSpecial,
            /// <summary>ИЖС (дома, районы)</summary>
            Houses,
            /// <summary>Первичка (тоже самое, что и вторичка [районы + площади])</summary>
            FirstHands,
        }

        public List<string> Create(Type type)
        {
            baseUrl = GetUrlByType(type);

            Excel sourceTable = ExcelHelper.LoadExcel(Pathes.steamingAssets + "/source_table.xlsx");

            specialRule = new();
            /*Для 1-комнатных проверка по площади: <35   35-42  >42

Для 2-комнатных:   <53   53-65  >65

Для 3-комнатных:  < 85  85-120  >120

Для 4 комнатных – фильтров нет

Для студий: <23  23-25  >25
             * 
             * */
            specialRule.AddRange(0, new Range("<35"));
            specialRule.AddRange(0, new Range("35-42"));
            specialRule.AddRange(0, new Range(">42"));

            specialRule.AddRange(1, new Range("<53"));
            specialRule.AddRange(1, new Range("53-65"));
            specialRule.AddRange(1, new Range(">65"));

            specialRule.AddRange(2, new Range("<85"));
            specialRule.AddRange(2, new Range("85-120"));
            specialRule.AddRange(2, new Range(">120"));

            specialRule.AddRange(4, new Range("<23"));
            specialRule.AddRange(4, new Range("23-25"));
            specialRule.AddRange(4, new Range(">25"));

            ExtractDistricts(sourceTable);
            ExtractRules(sourceTable);

            return CreateUrls(type);
        }

        private string GetUrlByType(Type type)
        {
            return type switch
            {
                Type.SaleDistrictArea => "https://spb.cian.ru/cat.php?deal_type=sale&district%5B0%5D={0}&engine_version=2&object_type%5B0%5D=1&offer_type=flat&totime=864000",
                Type.RentDistrictArea => "https://spb.cian.ru/cat.php?deal_type=rent&district%5B0%5D={0}&engine_version=2&offer_type=flat&only_flat=1&room1=1&totime=604800&type=4",
                Type.SaleRoomsDistrict => "https://spb.cian.ru/cat.php?deal_type=sale&district%5B0%5D={0}&engine_version=2&offer_type=flat&room0=1&totime=2592000",
                Type.SaleApartmentsSpecial => "https://spb.cian.ru/cat.php?apartment=1&deal_type=sale&engine_version=2&offer_type=flat&region=2&totime=2592000",
                Type.Houses => "https://spb.cian.ru/cat.php?deal_type=sale&district%5B0%5D={0}&engine_version=2&offer_type=suburban&object_type%5B0%5D=1",
                Type.FirstHands => "https://spb.cian.ru/cat.php?deal_type=sale&region=2&engine_version=2&object_type%5B0%5D=2&offer_type=flat&district%5B0%5D={0}",
                _ => throw new System.NotImplementedException("Failed to get url for cian type = " + type),
            };
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
        private List<string> CreateUrls(Type type)
        {
            List<string> urls = new List<string>();

            bool includeDistricts = 
                type == Type.SaleRoomsDistrict || 
                type == Type.SaleDistrictArea || 
                type == Type.RentDistrictArea || 
                type == Type.Houses || 
                type == Type.FirstHands;

            bool includeAreas = 
                type == Type.SaleDistrictArea ||
                type == Type.RentDistrictArea ||
                type == Type.SaleFirstSpecial ||
                type == Type.SaleApartmentsSpecial ||
                type == Type.FirstHands;

            if (includeDistricts)
            {
                foreach (string district in districts)
                {
                    if (includeAreas && rules.TryGetValue(district, out Rule rule))
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
            }
            else
            {
                for (int r = 0; r < roomsArgs.Length; r++)
                {
                    if (specialRule.ranges.TryGetValue(r, out List<Range> ranges))
                    {
                        foreach (Range range in ranges)
                        {
                            urls.Add(CreateUrl(baseUrl, r, range));
                        }
                    }
                    else
                    {
                        urls.Add(AppendRoomArgs(baseUrl, r, null));
                    }
                }
            }

            Debug.Log($"Type = {type}, urls ({urls.Count}):\n" + string.Join("\n", urls));
            return urls;
        }
        private string CreateUrl(string district, int rooms, Range range)
        {
            string url = string.Format(baseUrl, district);

            url = AppendRoomArgs(url, rooms, range);

            return url;
        }
        private string AppendRoomArgs(string url, int rooms, Range range)
        {
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