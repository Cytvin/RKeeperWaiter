using System;

namespace RKeeperWaiter.Models
{
    public class Modifier : ICloneable
    {
        private int _id;
        private int _code;
        private int _maxOneDish;
        private int _count;
        private decimal _price;
        private string _name;
        private bool _inMenu = false;
        private bool _selected;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public decimal Price => _price;
        public bool IsInMenu => _inMenu;
        public int MaxOneDish { get => _maxOneDish; set => _maxOneDish = value < 1 ? 1 : value; }
        public int Count => _count;
        public bool Selected => _selected;

        public Modifier(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }

        private Modifier(int id, int code, string name, decimal price, bool inMenu, int maxOneDish) 
        {
            _id = id;
            _code = code;
            _name = name;
            _price = price;
            _inMenu = inMenu;
            _maxOneDish = maxOneDish;
        }

        public void IncreaseCount()
        {
            if (_selected == false)
            {
                return;
            }

            if (_count >= _maxOneDish)
            {
                return;
            }

            _count++;
        }

        public void DecreaseCount()
        {
            if (_selected == false)
            {
                return;
            }

            if (_count <= 0)
            {
                return;
            }

            _count--;
        }

        public void Select()
        {
            if (_selected == true)
            {
                return;
            }

            _selected = true;
            IncreaseCount();
        }

        public void Deselect()
        {
            if (_selected == false)
            {
                return;
            }

            _selected = false;
            _count = 0;
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

        public object Clone()
        {
            return new Modifier(_id, _code, _name, _price, _inMenu, _maxOneDish);
        }
    }
}
