using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VendingMachineCodeFirst
{
    class Report
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GenerateReport(string filePath, List<Transaction> list)
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
            catch (Exception ex)
            {
                log.Error("GET REFILL failed");
                return "Last refill date missing";
            }
        }
    }
}