using System;
using System.IO;
using Newtonsoft.Json;
using VendingMachineCommon;
using System.Collections.Generic;
using VendingMachineCodeFirst.Utils;

namespace VendingMachineCodeFirst
{
    public class DataRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string filePath = FilePath.CurentStatePATH;
        private const string filePathAll = FilePath.AllStatePATH;
  
        public void PersistData(IList<Product> products)
        {
            WriteCurrentState(products);
            AppendCurrentState(products);
        }

        private void WriteCurrentState(IList<Product> products)
        {
            try
            {
                using (StreamWriter myFile = new StreamWriter(filePath))

                    foreach (Product p in products)
                    {
                        myFile.WriteLine(JsonConvert.SerializeObject(p));
                    }
                log.Info("Current state written in file");
            }
            catch (Exception)
            {
                log.Error("File error");
            }
        }

        private void AppendCurrentState(IList<Product> products)
        {
            try
            {
                foreach (Product p in products)
                {
                    File.AppendAllLines(filePathAll, new[] { JsonConvert.SerializeObject(p) });
                }
                log.Info("Current state appended");
            }
            catch (ArgumentNullException)
            {
                log.Error("File path/contents is null");
            }
            catch (IOException)
            {
                log.Error("Error occurred while opening the file");
            }
            catch (Exception)
            {
                log.Error("File Error when appending");
            }
        }

    }
}
