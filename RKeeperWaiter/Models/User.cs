using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class User
    {
        private int _id;
        private string _name;
        private int _code;
        private Guid _guid;

        public int Id => _id;
        public string Name => _name;
        public int Code => _code;
        public Guid Guid => _guid;

        public User (int id, string name, int code, Guid guid)
        {
            _id = id;
            _name = name;
            _code = code;
            _guid = guid;
        }
    }
}
