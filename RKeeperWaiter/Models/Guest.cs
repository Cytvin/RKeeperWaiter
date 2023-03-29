using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Guest
    {
        private string _label;

        public string Label { get { return _label; } }

        public Guest(string name) 
        {
            _label = name;
        }
    }
}
