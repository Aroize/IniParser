using System;

namespace IniParser.parser {
    public class ValidationException : Exception {
        public ValidationException(string error) 
            : base($"Expected value type, found {error}") 
        {
        }
    }
}