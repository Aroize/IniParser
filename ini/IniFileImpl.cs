using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IniParser.ini {
    public class IniFileImpl : IniFile {
        
        private readonly HashSet<Section> _sections = new HashSet<Section>(new SectionComparer());

        public bool AddSection(Section section) {
            if (_sections.Contains(section))
                return false;
            _sections.Add(section);
            return true;
        }
        
        public int GetInt(string sectionName, string fieldName) {
            foreach (var section in _sections.Where(section => section.Name == sectionName)) {
                if (section.GetInt(fieldName, out var result)) {
                    return result;
                }
                throw new ArgumentException($"There's no field with name {fieldName}");
            }
            throw new ArgumentException($"There's no section with name {sectionName}");
        }

        public string GetString(string sectionName, string fieldName) {
            foreach (var section in _sections.Where(section => section.Name == sectionName)) {
                if (section.GetString(fieldName, out var result)) {
                    return result;
                }
                throw new ArgumentException($"There's no field with name {fieldName}");
            }
            throw new ArgumentException($"There's no section with name {sectionName}");
        }

        public float GetFloat(string sectionName, string fieldName) {
            foreach (var section in _sections.Where(section => section.Name == sectionName)) {
                if (section.GetFloat(fieldName, out var result)) {
                    return result;
                }
                throw new ArgumentException($"There's no field with name {fieldName}");
            }
            throw new ArgumentException($"There's no section with name {sectionName}");
        }

        public override string ToString() {
            var builder = new StringBuilder();
            foreach (var section in _sections) {
                builder.Append(section);
                builder.Append('\n');
            }

            return builder.ToString();
        }
        
        private class SectionComparer : IEqualityComparer<Section> {
            public int GetHashCode(Section section) {
                return section.Name.GetHashCode();
            }

            public bool Equals(Section first, Section second) {
                return first.Name == second.Name;
            }
        }
    }
}