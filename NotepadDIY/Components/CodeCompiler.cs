using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NotepadDIY.Components
{
    static class CodeCompiler
    {
        static public void compileRunCSharp(string inputCodeText, FileInfo fileInfo)
        {

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            var Output = fileInfo.DirectoryName + @"\" + Path.GetFileNameWithoutExtension(fileInfo.Name) + ".exe";
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, inputCodeText);
            if (results.Errors.Count > 0)
            {
                string t = "";
                foreach (CompilerError err in results.Errors)
                {
                    t += err.ErrorNumber + ":" + err.ErrorText + "\n";
                }
                MessageBox.Show(t, "Build Fail");
            }
            else
            {
                Console.WriteLine(Output);
                Process.Start("cmd", "/c " + Output + " && timeout 5");
            }
        }
        static public void compileRunCPP(FileInfo fileInfo)
        {
            var outputfile = fileInfo.DirectoryName + Path.GetFileNameWithoutExtension(fileInfo.Name) + ".exe";
            Process.Start("cmd", "/k cmd /c" + " g++" + " -o " + outputfile + " " + fileInfo.FullName + " && " + outputfile + " && timeout 5");
        }
        static public void compileRunNodejs(FileInfo fileInfo)
        {
            Process.Start("cmd", "/k cmd /c node " + fileInfo.FullName + " && timeout 5");
        }
    }
}
