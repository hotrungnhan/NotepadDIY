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
    /// <summary>
    /// xml file should be C++_cpp_h.xml
    ///                    LanguageName_fileext_fileext etc;
    /// </summary>
    class XMLscriptItem
    {
       
        public string[] Ext { get; set; }
        public string Filter { get; set; }
        public string Path { get; set; }
        public XMLscriptItem(string[] ext, string filter, string Path)
        {
            this.Ext = ext;
            this.Filter = filter;
            this.Path = Path;
          
        }
        static public string getLanguageName(string FileNameWithExt)
        {
            return FileNameWithExt.Split('_')[0];
        }
        static public string getFilterExt(string FileNameWithExt)
        {
            var splitarr = FileNameWithExt.Split('_');
            var filter = splitarr[0] + "|";
            var ext = splitarr.Skip(1).Take(splitarr.Count() - 1);
            filter += "*.";
            filter += string.Join(";*.", ext);
            return filter;
        }
        static public string[] getExtList(string FileNameWithExt)
        {
            var splitarr = FileNameWithExt.Split('_');
            var ext = splitarr.Skip(1).Take(splitarr.Count() - 1);
            return ext.ToArray();
        }
    }
    static class LoadXMLScript
    {
        static bool firstload = true;
        static public Dictionary<string, XMLscriptItem> ScriptPathDict { get; set; } = new Dictionary<string, XMLscriptItem>();
        static public void LoadFile()
        {
            Console.WriteLine("wakeup");
            if (firstload == true)
            {
                try
                {
                    string[] files = Directory.GetFiles(@"Script", "*.xml");
                    foreach (string f in files)
                    {
                        var name = Path.GetFileNameWithoutExtension(f);
                        ScriptPathDict.Add(XMLscriptItem.getLanguageName(name), new XMLscriptItem(XMLscriptItem.getExtList(name), XMLscriptItem.getFilterExt(name), f));
                    }
                }
                catch (Exception err) { Console.WriteLine(err.Message); }
                firstload = false;
            }
        }
        static public bool isBuiltInLanguage(string lang)
        {
            switch (lang)
            {
                //For example, we will highlight the syntax of C# manually, although could use built-in highlighter
                case "Plain":
                case "Custom":
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
        static public Language getLangByExt(string lang)
        {
            switch (lang)
            {
                //For example, we will highlight the syntax of C# manually, although could use built-in highlighter
                case ".cs": return Language.CSharp;
                case ".vb": return Language.VB;
                case ".html": return Language.HTML;
                case ".xml": return Language.XML;
                case ".sql": return Language.SQL;
                case ".php": return Language.PHP;
                case ".js": return Language.JS;
                case ".lua": return Language.Lua;
                case ".json": return Language.JSON;
                default: return Language.Custom;
            }
        }
        static public string getXMLScriptPath(string lang)
        {
            if (ScriptPathDict.ContainsKey(lang))
            {
                return ScriptPathDict[lang].Path;
            }
            else return "";
        }
        static public string[] getXMLScriptExt(string lang)
        {
            if (ScriptPathDict.ContainsKey(lang))
            {
                return ScriptPathDict[lang].Ext;
            }
            else return null;
        }
    }
}
