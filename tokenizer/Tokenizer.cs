using System.Linq;

namespace IniParser.tokenizer {
    public class Tokenizer {
        private const string SpecialSymbols = "[]=;";

        private IniToken _prevToken = IniToken.Start;
        private readonly string[] _content;
        private int _arrayIndex;
        private int _stringIndex;

        public IniToken CurrentToken => _prevToken;

        public IniToken NextToken() {
            SkipSpace();
            if (_content.Length == _arrayIndex)
                return SaveCurrent(IniToken.End);
            switch (_content[_arrayIndex][_stringIndex]) {
                case '[': {
                    return SaveCurrent(IniToken.SectionOpen);
                }
                case ']': {
                    if (_prevToken != IniToken.SectionName)
                        throw new TokenizerException("name of section",_content[_arrayIndex], _stringIndex);
                    return SaveCurrent(IniToken.SectionClose);
                }
                case '=': {
                    if (_prevToken != IniToken.Variable)
                        throw new TokenizerException(_content[_arrayIndex], _stringIndex);
                    return SaveCurrent(IniToken.Equals);
                }
                case ';': {
                    _arrayIndex++;
                    _stringIndex = -1;
                    return SaveCurrent(IniToken.Comment);
                }
                default: {
                    switch (_prevToken) {
                        case IniToken.Equals:
                            return SaveCurrent(IniToken.Value);
                        case IniToken.Value:
                        case IniToken.SectionClose:
                        case IniToken.Comment:
                            return SaveCurrent(IniToken.Variable);
                        case IniToken.SectionOpen:
                            return SaveCurrent(IniToken.SectionName);
                    }
                    break;
                }
            }
            throw new TokenizerException(_content[_arrayIndex], _stringIndex);
        }

        public string GetValue() {
            var validSymbols = "_";
            if (_prevToken == IniToken.Value)
                validSymbols = validSymbols.Concat(".").ToString();
            var startIndex = _stringIndex - 1;
            while (_content[_arrayIndex].Length > _stringIndex &&
                   !SpecialSymbols.Contains(_content[_arrayIndex][_stringIndex]) &&
                   (char.IsLetterOrDigit(_content[_arrayIndex][_stringIndex]) || 
                    validSymbols.Contains(_content[_arrayIndex][_stringIndex]))) {
                _stringIndex++;
            }
            return _content[_arrayIndex].Substring(startIndex, _stringIndex - startIndex);
        }
        private IniToken SaveCurrent(IniToken currentToken) {
            _prevToken = currentToken;
            _stringIndex++;
            return currentToken;
        }
        public Tokenizer(string[] content) {
            _content = content;
        }
        private void SkipSpace() {
            if (_arrayIndex == _content.Length)
                return;
            while (_content[_arrayIndex].Length > _stringIndex && 
                   char.IsWhiteSpace(_content[_arrayIndex][_stringIndex])) {
                _stringIndex++;
            }
            if (_content[_arrayIndex].Length <= _stringIndex) {
                _arrayIndex++;
                _stringIndex = 0;
                SkipSpace();
            }
        }

        public void Stat(out string line, out int index) {
            line = _content[_arrayIndex];
            index = _stringIndex;
        }
    }
}