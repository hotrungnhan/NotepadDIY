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

    public partial class DebugControl : UserControl
    {

        private Process process;
        CompilerParameters parameters;
        string Output;
        public DebugControl()
        {
            InitializeComponent();

            process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.RedirectStandardError = true;

            parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.GenerateInMemory = false;
        }



        public string getRelativePath(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return Uri.UnescapeDataString(referenceUri.MakeRelativeUri(fileUri).ToString()).Replace('/', Path.DirectorySeparatorChar);
        }

        public void compileRunCSharp(string inputCodeText, FileInfo fileInfo)
        {
            errorTextBox.Text = "";

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();

            Output = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".exe";


            parameters.OutputAssembly = fileInfo.DirectoryName + @"\" + Output;


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

                Process.Start(parameters.OutputAssembly);
            }
        }
        public void compileRunCPP(FileInfo fileInfo)
        {
            string results = "";

            try
            {
                Output = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".exe";
                parameters.OutputAssembly = fileInfo.DirectoryName + @"\" + Output;

                process.StartInfo.FileName = @"g++.exe";
                process.StartInfo.Arguments = @"-o " + parameters.OutputAssembly + @" " + fileInfo.FullName;

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
                    Process.Start(parameters.OutputAssembly);
                }
                else
                {
                    errorTextBox.ForeColor = Color.Red;
                    errorTextBox.Text = results;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Install g++ as your environment first ");
            }
        }
        public void compileRunNodejs(FileInfo fileInfo)
        {
            string results = "";

            try
            {
                Output = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".exe";
                parameters.OutputAssembly = fileInfo.DirectoryName + @"\" + Output;

                process.StartInfo.FileName = @"node.exe";
                process.StartInfo.Arguments = " " + fileInfo.FullName;

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
                    Process.Start(parameters.OutputAssembly);
                }
                else
                {
                    errorTextBox.ForeColor = Color.Red;
                    errorTextBox.Text = results;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Install nodejs as your environment first ");
            }
        }
    }
}