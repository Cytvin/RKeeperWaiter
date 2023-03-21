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

        public int Id { get { return _id; } }
        public int Code { get { return _code; } }
        public string Name { get { return _name; } }

        public Hall(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _tables = new List<Table>();
        }

        public void AddTable(Table table)
        {
            _tables.Add(table);
        }

        public IEnumerable<Table> GetTables() 
        { 
            return _tables; 
        }   
    }
}
