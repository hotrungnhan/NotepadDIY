using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;


namespace NotepadDIY.Components
{
    
    public partial class CompileRun : UserControl
    {
        private string absolutePath;
        private string relativePath;
        //private string relativeCPPCompiler;
        public CompileRun()
        {
            absolutePath = @"D:\Code\IT008\NodepadDIY\NotepadDIY\bin\Debug\CPPTextCode.c";
            InitializeComponent();
            relativePath = getRelativePath(@"D:\Code\IT008\NodepadDIY\NotepadDIY\bin\Debug\CPPTextCode.c", @"D:\Code\IT008\NodepadDIY\NotepadDIY\bin\Debug\");
            //relativeCPPCompiler = getRelativePath(@"D:\MingW\bin\g++.exe", @"D:\MingW\bin");
        }
        
        public string getRelativePath(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return Uri.UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString()).Replace('/', Path.DirectorySeparatorChar);
        }
        public void compileRunCSharp(string inputCodeText)
        {
            errorTextBox.Text = "";

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "ForCSharp.exe";



            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;

            CompilerResults results = icc.CompileAssemblyFromSource(parameters, inputCodeText);

            if (results.Errors.Count > 0)
            {
                errorTextBox.ForeColor = Color.Red;
                foreach (CompilerError CompErr in results.Errors)
                {
                    errorTextBox.Text = errorTextBox.Text +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                //Successful Compile
                errorTextBox.ForeColor = Color.Blue;
                errorTextBox.Text = "Success!";
                Process.Start(Output);
            }
        }
        public void compileRunCPP(string inputCodeText)
        {            
            string results = "";

            try
            {
                if (File.Exists(this.absolutePath))
                    File.Delete(this.absolutePath);

                using (FileStream fs = File.Create(this.absolutePath))
                {
                    byte[] codeText = new UTF8Encoding(true).GetBytes(inputCodeText);
                    fs.Write(codeText, 0, codeText.Length);
                }

                Process process = new Process();
                process.StartInfo.FileName = @"g++.exe"; 
                process.StartInfo.Arguments = @"-o CPPRun " + relativePath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                using (StreamReader sr = process.StandardError)
                {
                    if (sr.BaseStream.CanRead)
                        results = sr.ReadToEnd();

                }

                process.Close();

                if (String.IsNullOrWhiteSpace(results))
                {
                    errorTextBox.ForeColor = Color.Blue;
                    errorTextBox.Text = "Success";
                    Process.Start("CPPRun").WaitForExit();
                }
                else
                {
                    errorTextBox.ForeColor = Color.Red;
                    errorTextBox.Text = results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
