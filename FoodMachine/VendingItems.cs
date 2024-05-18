using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodMachine
{
    class VendingItem
    {
        public string name;
        public double price;

        public VendingItem(string item, double cost)
        {
            name = item;
            price = cost;
        }
    }
}
