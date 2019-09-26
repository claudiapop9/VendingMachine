using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VendingMachineCodeFirst.Utils;

namespace VendingMachineCodeFirst
{
    public class ReportRepository
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string filePath = FilePath.ReportPATH;

        public string GenerateReport(List<Transaction> list)
        {
            try
            {
                using (StreamWriter myFile = new StreamWriter(filePath))
                {
                    string str = "Report " + DateTime.Now;
                    str += GetMostBought(list) + "\n" + GetLastRefillDate(list);
                    myFile.WriteLine(str);
                    log.Info("Report written successfully");
                    return str;
                }
            }
            catch (Exception)
            {
                log.Error("Writing report");
                return null;
            } 
        }

        private string GetMostBought(List<Transaction> list)
        {
            string str = "Most Bought Products:\n";
            var result = list.GroupBy(p => p.ProductId)
                .ToDictionary(x => x.Key, x => x.Count());

            foreach (var key in result)
            {
                str += key.Key.ToString();
                str += " \n";
            }

            return str;
        }

        private string GetLastRefillDate(List<Transaction> list)
        {
            try
            {
                string str = "Last Refill:\n";
                var result = list.Where(t => t.Type == "REFILL").Select(t => (t.Date)).OrderByDescending(t => t.Value);
                foreach (var key in result)
                {
                    str += key.ToString();
                    str += " \n";
                }
                return str;
            }
            catch (Exception)
            {
                log.Error("GET REFILL failed");
                return "Last refill date missing";
            }
        }
    }
}