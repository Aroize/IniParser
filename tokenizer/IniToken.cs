namespace IniParser {
    public enum IniToken {
        Start,
        SectionOpen,
        SectionName,
        SectionClose,
        Value,
        Variable,
        Equals,
        Comment,
        End
    }
}