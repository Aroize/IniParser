using System.Xml.Serialization;

namespace IniParser.ini {
    public interface IniFile {
        int GetInt(string sectionName, string fieldName);

        float GetFloat(string sectionName, string fieldName);

        string GetString(string sectionName, string fieldName);
    }
}