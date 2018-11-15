﻿using System;
using System.Collections.Generic;
using System.Text;

namespace oop_principles
{
    class Engine
    {
        private int dimension;
        private string fuel;
        private double price;

        public Engine() {}

        public Engine(int dimension,string fuel,float price)
        {
            this.dimension = dimension;
            this.fuel = fuel;
            this.price = cost();          
        }

        public double cost() {
            try
            {
                double price = dimension * 1.05;
                if (this.fuel == "gas")
                {
                    price += 500;
                }
                else
                {
                    price += 300;
                }
                return price;
            }
            catch (Exception) { }

            return 2500;
        }

        public int dimProperty {
            get { return this.dimension; }
            set { this.dimension = value; }
        }

        public string fuelProperty {
            get { return this.fuel; }
            set { this.fuel = value; }
        }

        public double priceProperty {
            get { return this.price; }
            set { this.price = value; }
        }

        public override string ToString()
        {
            string str = "Engine properties:\n";
            str += "engine size: " + this.dimension.ToString()+" cm^3\n";
            str += "fuel: " + this.fuel.ToString();
            return str;
        }
    }
}
