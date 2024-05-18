using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoodMachine
{
    class Account
    {
        public string username;
        public string PIN;
        public int accessLevel;
        public double balance;

        public Account(string name, string id, int access, double bal)
        {
            username = name;
            PIN = id;
            accessLevel = access;
            balance = bal;
        }
    }
}
