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
                
        private string currentfilepath;
        private string currentfilename;
        
        public DebugControl()
        {
            
            
            setUpPath();

            InitializeComponent();

        }
        public void setUpPath()
        {
            currentfilename = "NewCode";
            currentfilepath = Path.GetTempPath() + currentfilename + @"\";
            
            //currentfilepath = XXXX;
        }
        public void setUpPath(string currentname)
        {
            currentfilepath = Path.GetTempPath() + currentfilename + @"\";
        }
        public void setUpPath(string currentpath, string currentname)
        {
            currentfilepath = currentpath;
            currentfilename = currentname;
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

            
            Directory.CreateDirectory(currentfilepath);

            if (File.Exists(currentfilepath + currentfilename + @".cs"))
                File.Delete(currentfilepath + currentfilename + @".cs");

            using (FileStream fs = File.Create(currentfilepath + currentfilename + @".cs"))
            {
                byte[] codeText = new UTF8Encoding(true).GetBytes(inputCodeText);
                fs.Write(codeText, 0, codeText.Length);
            }

            string Output = currentfilename + ".exe";                     

            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();            
            parameters.GenerateExecutable = true;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = currentfilepath + Output;
            //parameters.CompilerOptions = currentfilepath + Output;   
                        
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

                Process.Start(currentfilepath + Output);                   
            }
        }
        public void compileRunCPP(string inputCodeText)
        {
            
            string results = "";

            Directory.CreateDirectory(currentfilepath);

           

            try
            {
                if (File.Exists(currentfilepath + currentfilename + @".c"))
                    File.Delete(currentfilepath + currentfilename + @".c");

                using (FileStream fs = File.Create(currentfilepath + currentfilename + @".c"))
                {
                    byte[] codeText = new UTF8Encoding(true).GetBytes(inputCodeText);
                    fs.Write(codeText, 0, codeText.Length);
                }

                

                Process process = new Process();
                //process.StartInfo.WorkingDirectory = cppcompilerpath;
                process.StartInfo.FileName = @"g++.exe";
                process.StartInfo.Arguments = @"-o " + currentfilepath + currentfilename + @" " + currentfilepath + currentfilename + @".c";
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


                    Process.Start(currentfilepath + currentfilename + ".exe").WaitForExit();
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
