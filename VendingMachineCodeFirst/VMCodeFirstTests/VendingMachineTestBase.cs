using VendingMachineCodeFirst;

namespace VMCodeFirstTests
{
    class VendingMachineTestBase
    {
        protected readonly VendMachineDbContext context;
        public VendingMachineTestBase()
        {
            context = new VendMachineDbContext();
            
        }
    }
}