using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VendingMachineCodeFirst;

namespace BusinessLogic
{
    class PaymentDTO
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IPayment FromMap(string json)
        {
            IPayment payment = null;
            if (json.Contains("Card"))
            {
                try
                {
                    payment = JsonConvert.DeserializeObject<CardService>(json);
                    return payment;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }else if (json.Contains("Money"))
            {
                try
                {
                    List<CashMoney> cash = JsonConvert.DeserializeObject<List<CashMoney>>(json);
                    payment = new CashService(cash);
                    return payment;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return payment;
        }
    }
}
