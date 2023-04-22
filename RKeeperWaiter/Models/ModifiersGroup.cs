using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RKeeperWaiter.Models
{
    public class ModifiersGroup
    {
        private int _id;
        private int _code;
        private string _name;
        private List<Modifier> _modifiers;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<Modifier> Modifiers => _modifiers;

        public ModifiersGroup(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _modifiers = new List<Modifier>();
        }

        public void InsertModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
        }
    }
}
