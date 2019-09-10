
using System.Collections.Generic;

namespace SolidViolation
{
    class Shelter
    {
        //Dependency Inv P
        List<Dog> dogs;
        List<Fish> fishes;

        public void AddDog(Dog d)
        {
            dogs.Add(d);
        }

        public void AddFish(Fish f)
        {
            fishes.Add(f);
        }

        //Open/Close principle violation
        public void Adopt()
        {
            Dog dog = new Dog("Mike");
            dogs.Remove(dog);
            System.Console.WriteLine($"You adopted {dog}");
        }

        //Single Resposability principle violation-- class level 
        public void ManageStoredData()
        {
            System.Console.WriteLine("data management");

        }
        
        public void StoreClientDetails()
        {
            System.Console.WriteLine("Clients details are stored");
        }

        


    }
}
