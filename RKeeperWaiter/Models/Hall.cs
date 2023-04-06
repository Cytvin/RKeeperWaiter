using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class Hall
    {
        private int _id;
        private int _code;
        private string _name;

        private List<Table> _tables;

        public int Id => _id; 
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<Table> Tables => _tables;

        public Hall(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _tables = new List<Table>();
        }

        public void InsertTable(Table table)
        {
            _tables.Add(table);
        }
    }
}
