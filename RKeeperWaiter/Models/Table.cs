﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Table
    {
        private int _id;
        private int _code;
        private string _name;
        private int _maxGuests;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public int MaxGuests => _maxGuests;

        public Table(int id, int code, string name, int maxGuests) 
        {
            _id = id;
            _code = code;
            _name = name;
            _maxGuests = maxGuests;
        }
    }
}
