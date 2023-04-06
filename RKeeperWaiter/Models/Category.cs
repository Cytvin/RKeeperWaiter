using System;

namespace RKeeperWaiter.Models
{
    public class Category
    {
        private int _id;
        private int _parentId;
        private string _name;

        public int Id => _id;
        public int ParentId => _parentId;
        public string Name => _name; 

        public Category(int id, string name, int parentId) 
        {
            _id = id;
            _name = name;
            _parentId = parentId;
        }
    }
}
