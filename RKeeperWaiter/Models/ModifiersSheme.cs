using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class ModifiersSheme
    {
        private int _id;
        private int _code;
        private string _name;
        private List<ModifiersGroup> _modifiersGroups;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<ModifiersGroup> ModifiersGroups => _modifiersGroups;

        public ModifiersSheme(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _modifiersGroups = new List<ModifiersGroup>();
        }

        public void InsertModifiersGroup(ModifiersGroup modifiersGroup)
        {
            _modifiersGroups.Add(modifiersGroup);
        }
    }
}
