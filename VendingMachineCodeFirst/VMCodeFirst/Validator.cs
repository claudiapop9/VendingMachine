using System;
using System.Linq;
using VendingMachineCommon;

namespace VendingMachineCodeFirst
{
    class Validator
    {
        private VendMachineDbContext context = new VendMachineDbContext();
        internal bool isIdValid(string id)
        {
            try
            {
                int productId = Int32.Parse(id);
                Product p = context.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                return p != null;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
