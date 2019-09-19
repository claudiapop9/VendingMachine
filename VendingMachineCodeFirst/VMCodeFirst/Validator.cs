using System;
using System.Linq;

namespace VendingMachineCodeFirst
{
    class Validator
    {
        private VendMachineDbContext context = new VendMachineDbContext();
        internal bool isIdValid(int id)
        {
            try
            {
                Product p = context.Products.Where(x => x.ProductId == id).FirstOrDefault();
                return p != null;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
