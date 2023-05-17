using System;
using System.Collections.Generic;
using System.Linq;

namespace RKeeperWaiter.Models
{
    public class ModifiersSheme : ICloneable
    {
        private int _id;
        private int _code;
        private string _name;
        private List<ModifiersGroup> _requiredModifiersGroups;
        private List<ModifiersGroup> _optionalModifiersGroups;

        public int Id => _id;
        public int Code => _code;
        public string Name => _name;
        public IEnumerable<ModifiersGroup> RequiredModifiersGroups => _requiredModifiersGroups;
        public IEnumerable<ModifiersGroup> OptionalModifiersGroups => _optionalModifiersGroups;
        public IEnumerable<Modifier> SelectedModifiers => GetSelectedModifiers();

        public ModifiersSheme(int id, int code, string name)
        {
            _id = id;
            _code = code;
            _name = name;
            _requiredModifiersGroups = new List<ModifiersGroup>();
            _optionalModifiersGroups = new List<ModifiersGroup>();
        }

        private ModifiersSheme(int id, int code, string name, List<ModifiersGroup> requiredModifiersGroups, List<ModifiersGroup> optionalModifiersGroups)
        {
            _id = id;
            _code = code;
            _name = name;
            _requiredModifiersGroups = requiredModifiersGroups.Select(g => (ModifiersGroup)g.Clone()).ToList();
            _optionalModifiersGroups = optionalModifiersGroups.Select(g => (ModifiersGroup)g.Clone()).ToList();
        }

        public void InsertRequiredModifiersGroup(ModifiersGroup modifiersGroup)
        {
            _requiredModifiersGroups.Add(modifiersGroup);
        }

        public void InsertOptionalModifiersGroup(ModifiersGroup modifiersGroup)
        {
            _optionalModifiersGroups.Add(modifiersGroup);
        }

        public object Clone()
        {
            return new ModifiersSheme(_id, _code, _name, _requiredModifiersGroups, _optionalModifiersGroups);
        }

        private IEnumerable<Modifier> GetSelectedModifiers()
        {
            List<Modifier> modifiers = new List<Modifier>();

            foreach(ModifiersGroup modifiersGroup in _requiredModifiersGroups)
            {
                modifiers.AddRange(modifiersGroup.Modifiers.Where(m => m.Selected));
            }

            foreach (ModifiersGroup modifiersGroup in _optionalModifiersGroups)
            {
                modifiers.AddRange(modifiersGroup.Modifiers.Where(m => m.Selected));
            }

            return modifiers;
        }
    }
}
