using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using FastColoredTextBoxNS;
namespace NotepadDIY.Components
{
    static class LoadXMLScript
    {
        static bool isLoad = false;
        static public Dictionary<string, string> ScriptPathDict { get; set; } = new Dictionary<string, string>();
        static public void LoadFile()
        {
            if (isLoad == true)
            {
                try
                {
                    string[] files = Directory.GetFiles(@"Script", "*.xml");
                    foreach (string f in files)
                    {
                        var name = Path.GetFileNameWithoutExtension(f);
                        ScriptPathDict.Add(name, f);
                    }
                }
                catch (Exception err) { Console.WriteLine(err.Message); }
                isLoad = true;
            }
        }
        static public bool isBuiltInLanguage(string lang)
        {
            
            switch (lang)
            {
                //For example, we will highlight the syntax of C# manually, although could use built-in highlighter
                case "Plain":
                case "CSharp":
                case "VB":
                case "HTML":
                case "XML":
                case "SQL":
                case "PHP":
                case "JS":
                case "Lua":
                case "JSON":
                    return true;
            }
            return false;
        }
        static public Language getBuiltInLanguage(string lang)
        {
            switch (lang)
            {
                //For example, we will highlight the syntax of C# manually, although could use built-in highlighter
                case "CSharp": return Language.CSharp;
                case "VB": return Language.VB;
                case "HTML": return Language.HTML;
                case "XML": return Language.XML;
                case "SQL": return Language.SQL;
                case "PHP": return Language.PHP;
                case "JS": return Language.JS;
                case "Lua": return Language.Lua;
                case "JSON": return Language.JSON;
                default: return Language.Custom;
            }
        }
        static public string getXMLScriptPath(string lang)
        {
            if (ScriptPathDict.ContainsKey(lang))
            {
                return ScriptPathDict[lang];
            }
            else return "";
        }
    }
}
