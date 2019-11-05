using System;

namespace IniParser.parser {
    public class ParserException : TokenizerException {
        public ParserException(IniToken correctToken, string line, int index) 
            : base($"{correctToken}", line, index) 
        {
        }
    }
}