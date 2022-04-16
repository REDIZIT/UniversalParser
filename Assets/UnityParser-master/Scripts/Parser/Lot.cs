using System;
using System.Collections.Generic;

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
    public interface ILotContainer
    {
        IEnumerable<Lot> EnumerateLots();
    }
    public class LotContainer<T> : Lot, ILotContainer where T : Lot
    {
        public List<T> lots = new List<T>();

        public IEnumerable<Lot> EnumerateLots()
        {
            foreach (T lot in lots)
            {
                yield return lot;
            }
        }
    }
}