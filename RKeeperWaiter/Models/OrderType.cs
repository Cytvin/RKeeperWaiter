using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class OrderType
    {
        private int _id;
        private int _code;
        private string _name;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;

        public OrderType(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }
    }
}
