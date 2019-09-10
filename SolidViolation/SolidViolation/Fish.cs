namespace SolidViolation
{
    class Fish : IAnimal
    {
        //Single Resposability principle violation-- method level
        public void Eat()
        {
            System.Console.WriteLine("fish eat");
                     
            System.Console.WriteLine("One thing..");
            
            //+ is doing another thing...
            //+ and another...
            //+ and another...
            // and another...

        }

        //--Liskov Substitution Principle
        public void Run()
        {
            System.Console.WriteLine("Fish can't run");
        }

        public void Sleep()
        {
            System.Console.WriteLine("The fish is sleeping");
        }
    }
}
