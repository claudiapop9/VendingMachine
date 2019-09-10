using System;

namespace SolidViolation
{
    class Dog : IAnimal
    {
        public String name;
        public Dog(){}

        public Dog(string name)
        {
            this.name = name;
        }

        public void Eat()
        {
            System.Console.WriteLine("The dog is eating");
        }

        public void Run()
        {
            Console.Write("Dogs run fast.");
        }

        public void Sleep()
        {
            System.Console.WriteLine("The dog is sleeping");
        }
    }
        
}
