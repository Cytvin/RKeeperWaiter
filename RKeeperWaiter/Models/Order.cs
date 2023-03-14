using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Order
    {
        private int _id;
        private int _visitId;
        private Guid _guid;
        private string _name;

        public int Id { get { return _id; } }
        public int VisitId { get { return _visitId; } }
        public Guid Guid { get { return _guid; } }
        public string Name { get { return _name; } }

        public Order (int id, int visitId, Guid guid, string name)
        {
            _id = id;
            _visitId = visitId;
            _guid = guid;
            _name = name;
        }
    }
}
