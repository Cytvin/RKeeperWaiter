using System;
using System.Collections.Generic;
using System.Linq;

namespace RKeeperWaiter.Models
{
    public class ModifiersGroup : ICloneable
    {
        private int _id;
        private int _code;
        private string _name;
        private List<ModifiersGroup> _internalGroups;
        private List<Modifier> _modifiers;
        private int _upLimit;
        private int _downLimit;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<Modifier> Modifiers => _modifiers;
        public IEnumerable<ModifiersGroup> ModifiersGroups => _internalGroups;
        public int UpLimit { get => _upLimit; set => _upLimit = value < 0 ? 0 : value; }
        public int DownLimit { get => _downLimit; set => _downLimit = value < 0 ? 0 : value; }

        public ModifiersGroup(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _internalGroups = new List<ModifiersGroup>();
            _modifiers = new List<Modifier>();
        }

        private ModifiersGroup(int id, int code, string name, List<ModifiersGroup> modifiersGroups,
            List<Modifier> modifiers, int upLimit, int downLimit)
        {
            _id = id;
            _code = code;
            _name = name;
            _internalGroups = modifiersGroups.Select(g => (ModifiersGroup)g.Clone()).ToList();
            _modifiers = modifiers.Select(m => (Modifier)m.Clone()).ToList();
            _upLimit = upLimit;
            _downLimit = downLimit;
        }

        private bool IsInLimit()
        {
            bool result = false;
            int modifiersCount = 0;

            foreach (Modifier modifier in _modifiers) 
            {
                modifiersCount += modifier.Count;
            }

            if (modifiersCount >= _downLimit && modifiersCount <= _upLimit)
            {
                result = true;
            }

            return result;
        }

        public void InsertModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void InsertGroup(ModifiersGroup modifiersGroup)
        {
            _internalGroups.Add(modifiersGroup);
        }

        public object Clone()
        {
            return new ModifiersGroup(_id, _code, _name, _internalGroups, _modifiers, _upLimit, _downLimit);
        }
    }
}
