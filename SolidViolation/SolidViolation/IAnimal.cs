
namespace SolidViolation
{
    /* Interface Segregation Principle
     -should be more interfaces
     -interface for run, fly, swim..etc
    */
    public interface IAnimal
    {
        void Run();
        void Eat();
        void Sleep();
    }
}
