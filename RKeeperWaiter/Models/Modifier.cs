using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Modifier
    {
        private int _id;
        private int _code;
        private string _name;
        private decimal _price;
        private bool _inMenu = false;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public decimal Price => _price;
        public bool IsInMenu => _inMenu;

        public Modifier(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                return;
            }

            _inMenu = true;
            _price = Math.Round(price / 100, 2);
        }
    }
}
