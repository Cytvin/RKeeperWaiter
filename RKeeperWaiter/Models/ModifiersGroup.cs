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
        private List<ModifiersGroup> _internalGroups;
        private List<Modifier> _modifiers;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<Modifier> Modifiers => _modifiers;
        public IEnumerable<ModifiersGroup> ModifiersGroups => _internalGroups;

        public ModifiersGroup(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _internalGroups = new List<ModifiersGroup>();
            _modifiers = new List<Modifier>();
        }

        public void InsertModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void InsertGroup(ModifiersGroup modifiersGroup)
        {
            _internalGroups.Add(modifiersGroup);
        }
    }
}
