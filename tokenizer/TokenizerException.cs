using System;
using System.Text;

namespace IniParser {
    [Serializable]
    public class TokenizerException : Exception {
        public TokenizerException(string errorString, int errorIndex) 
            : base(new StringBuilder()
                .Append('\n')
                .Append(errorString)
                .Append('\n')
                .Append('~', errorIndex)
                .Append('^').ToString()) 
        {
        }
        
        public TokenizerException(string expected, string errorString, int errorIndex)
            : base(new StringBuilder("\nExpected : ")
                .Append(expected)
                .Append("\nFound: ")
                .Append(errorString)
                .Append('\n')
                .Append('~', errorIndex + 7)
                .Append('^').ToString())
        {}
    }
}