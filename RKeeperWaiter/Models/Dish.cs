﻿using System;
using System.Collections.Generic;

namespace RKeeperWaiter.Models
{
    public class Dish : ICloneable
    {
        private int _id;
        private Guid _guid;
        private string _name;
        private bool _inMenu;
        private decimal _price;
        private int _parentId;
        private Course _course;
        private ModifiersSheme _modifiersSheme;
        private List<Modifier> _modifiers;

        public bool InMenu => _inMenu;
        public int Id => _id;
        public int ParentId => _parentId;
        public Guid Guid => _guid;
        public string Name => _name;
        public decimal Price => _price;
        public Course Course { get => _course; set => _course = value; }
        public string Seat { get; set; }
        public ModifiersSheme ModifiersSheme { get => _modifiersSheme; set => _modifiersSheme = value; }
        public IEnumerable<Modifier> Modifiers => _modifiers;

        public Dish(int id, Guid guid, string name, int parent) 
        {
            _id = id;
            _guid = guid;
            _name = name;
            _inMenu = false;
            _parentId = parent;
            _course = Course.Empty;
            _modifiers = new List<Modifier>();
        }

        public void SetPrice(decimal price)
        {
            if (price <= 0)
            {
                return;
            }

            _inMenu = true;
            _price = Math.Round(price / 100, 2);
        }

        public void InsertModifier(Modifier modifier)
        {
            if (_modifiers.Contains(modifier))
            {
                return;
            }

            _modifiers.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        public object Clone() => MemberwiseClone();
    }
}
