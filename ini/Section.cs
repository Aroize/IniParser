using System;
using System.Collections.Generic;
using System.Text;

namespace IniParser.ini {
    public class Section {
        public readonly string Name;

        private readonly Dictionary<string, int> _integers = new Dictionary<string, int>();
        private readonly Dictionary<string, float> _floats = new Dictionary<string, float>();
        private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();

        public Section(string name) {
            Name = name;
        }

        public bool AddInt(string key, int value) {
            if (_integers.ContainsKey(key))
                return false;
            _integers.Add(key, value);
            return true;
        }

        public bool AddFloat(string key, float value) {
            if (_floats.ContainsKey(key))
                return false;
            _floats.Add(key, value);
            return true;
        }

        public bool AddString(string key, string value) {
            if (_strings.ContainsKey(key))
                return false;
            _strings.Add(key, value);
            return true;
        }

        public bool GetInt(string key, out int result) {
            return _integers.TryGetValue(key, out result);
        }

        public bool GetFloat(string key, out float result) {
            return _floats.TryGetValue(key, out result);
        }

        public bool GetString(string key, out string result) {
            return _strings.TryGetValue(key, out result);
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.Append($"[{Name}]\n");
            foreach (var keyValuePair in _integers) {
                builder.Append($"{keyValuePair.Key}={keyValuePair.Value}\n");
                int value;
                var key = keyValuePair.Key;
                _integers.TryGetValue(keyValuePair.Key, out value);
            }

            foreach (var keyValuePair in _floats) {
                builder.Append($"{keyValuePair.Key}={keyValuePair.Value}\n");
            }
            
            foreach (var keyValuePair in _strings) {
                builder.Append($"{keyValuePair.Key}={keyValuePair.Value}\n");
            }
            return builder.ToString();
        }
    }
}