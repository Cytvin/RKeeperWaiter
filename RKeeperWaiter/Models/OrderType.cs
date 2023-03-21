﻿using System;
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

        public int Id { get { return _id; } }
        public int Code { get { return _code; } }
        public string Name { get { return _name; } }

        public OrderType(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
        }
    }
}
