using System;

namespace RKeeperWaiter
{
    public class Category
    {
        private int _id;
        private int _parentId;
        private string _name;

        public int Id { get { return _id; } }
        public int ParentId { get { return _parentId; } }
        public string Name { get { return _name; } }

        public Category(int id, string name, int parentId) 
        {
            _id = id;
            _name = name;
            _parentId = parentId;
        }
    }
}
