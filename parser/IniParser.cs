using System;
using System.IO;
using System.Text.RegularExpressions;
using IniParser.ini;
using IniParser.tokenizer;

namespace IniParser.parser {
    public class IniParser {

        private Tokenizer _tokenizer;
        
        private const string IniExtension = ".ini";

        public IniFile Parse(string filePath) {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new ArgumentNullException(nameof(filePath), "is null, empty or file doesn't exist");
            if (!CheckExtension(filePath)) {
                throw new ArgumentException("Provided file's path isn't INI file");
            }
            var content = File.ReadAllLines(filePath);
            
            _tokenizer = new Tokenizer(content);
            _tokenizer.NextToken();
            var file = new IniFileImpl();
            while (_tokenizer.CurrentToken != IniToken.End) {
                SkipComments();
                CheckOrThrow(IniToken.SectionOpen);
                var parsedSection = ParseSection();
                file.AddSection(parsedSection);
            }
            return file;
        }

        private Section ParseSection() {
            _tokenizer.NextToken();
            CheckOrThrow(IniToken.SectionName);
            var sectionName = _tokenizer.GetValue();
            var currentSection = new Section(sectionName);
            _tokenizer.NextToken();
            CheckOrThrow(IniToken.SectionClose);
            _tokenizer.NextToken();
            while (_tokenizer.CurrentToken != IniToken.SectionOpen && _tokenizer.CurrentToken != IniToken.End) {
                CheckOrThrow(IniToken.Variable);
                var currentField = _tokenizer.GetValue();
                _tokenizer.NextToken();
                CheckOrThrow(IniToken.Equals);
                _tokenizer.NextToken();
                CheckOrThrow(IniToken.Value);
                var value = _tokenizer.GetValue();
                var valueType = ValidateValue(value);
                switch (valueType) {
                    case ValueType.Integer: {
                        var result = int.Parse(value);
                        currentSection.AddInt(currentField, result);
                        break;
                    }
                    case ValueType.Float: {
                        var result = float.Parse(value);
                        currentSection.AddFloat(currentField, result);
                        break;
                    }
                    case ValueType.String: {
                        currentSection.AddString(currentField, value);
                        break;
                    }
                }
                _tokenizer.NextToken();
                SkipComments();
            }

            return currentSection;
        }

        private static ValueType ValidateValue(string value) {
            if (Regex.IsMatch(value, "[A-Za-z_\\.]+")) {
                return ValueType.String;
            }

            if (Regex.IsMatch(value, "\\-{0,1}[0-9]+\\.[0-9]")) {
                return ValueType.Float;
            }

            if (Regex.IsMatch(value, "\\-{0,1}[0-9]")) {
                return ValueType.Integer;
            }
            throw new ValidationException(value);
        }

        private void CheckOrThrow(IniToken token) {
            if (_tokenizer.CurrentToken == token) return;
            _tokenizer.Stat(out var line, out var index);
            throw new ParserException(IniToken.SectionName,line, index);
        }

        private void SkipComments() {
            while (_tokenizer.CurrentToken == IniToken.Comment) {
                _tokenizer.NextToken();
            }
        }
        
        private static bool CheckExtension(string filePath) {
            return IniExtension == Path.GetExtension(filePath);
        }
    }
}