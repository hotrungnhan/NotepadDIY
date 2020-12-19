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
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using Microsoft.VisualC;

namespace TestCompile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "Out.exe";



            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;

            CompilerResults results = icc.CompileAssemblyFromSource(parameters, this.fastColoredTextBox1.Text);

            if (results.Errors.Count > 0)
            {
                textBox2.ForeColor = Color.Red;
                foreach (CompilerError CompErr in results.Errors)
                {
                    textBox2.Text = textBox2.Text +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
            }
            else
            {
                //Successful Compile
                textBox2.ForeColor = Color.Blue;
                textBox2.Text = "Success!";
                Process.Start(Output);
            }
        }

        private void runCPPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\MingW\bin\gcc.exe");
            //startInfo.Arguments = fastColoredTextBox1.Text;


            // Application path and command line arguments
            string ApplicationPath = @"D:\MingW\bin\gcc.exe";
            
            string ApplicationArguments = "D:\\testgcc.cpp";

            // Create a new process object
            Process ProcessObj = new Process();

            // StartInfo contains the startup information of
            // the new process
            ProcessObj.StartInfo.FileName = ApplicationPath;
            ProcessObj.StartInfo.Arguments = ApplicationArguments;

            // These two optional flags ensure that no DOS window
            // appears
            ProcessObj.StartInfo.UseShellExecute = false;
            ProcessObj.StartInfo.CreateNoWindow = true;

            // This ensures that you get the output from the DOS application
            ProcessObj.StartInfo.RedirectStandardOutput = true;

            // Start the process
            ProcessObj.Start();

            // Wait that the process exits
            ProcessObj.WaitForExit();

            // Now read the compile output of the DOS application
            string Result = ProcessObj.StandardOutput.ReadToEnd();

            Process.Start(Result);
        }
        private void runCPP_Click(object sender, EventArgs e)
        {
            string path = @"D:\Code\IT008\NodepadDIY\TestCompile\bin\Debug\testgcc.c";
            string results = "";

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (FileStream fs = File.Create(path))
                {
                    byte[] codeText = new UTF8Encoding(true).GetBytes(fastColoredTextBox1.Text);
                    fs.Write(codeText, 0, codeText.Length);
                }

                Process process = new Process();
                process.StartInfo.FileName = @"C:\WINDOWS\system32\cmd.exe"; ;
                
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                
                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {                        
                        sw.WriteLine(@"gcc -o AAAAtest testgcc.c");
                        //sw.WriteLine(@"AAAAtest");
                        StreamReader sr = process.StandardError;


                       
                        results = sr.ReadToEnd();

                        if (String.IsNullOrEmpty(results))
                        {
                            textBox2.Text = results;
                            sw.WriteLine(@"AAAAtest");
                        }
                        else
                        {

                            textBox2.Text = results;
                        }






                    };
                    sw.Close();
                    
                }

                
                //using (StreamReader sr = process.StandardOutput)
                //{
                //    if (sr.BaseStream.CanRead)
                //        results = sr.ReadToEnd();

                //}
                process.WaitForExit();


            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            
            

            
            
            
        }
    }

}
