using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
namespace YottaCompilerTool
{

    public partial class Form1 : Form
    {
        const string BuildHexPath = @"\build\bbc-microbit-classic-gcc\source\microbit-samples-combined.hex";
        const string DefultPath = @"C:\Users\user\microbit-samples";
        const string DefultYottaPath = @"C:\yotta\workspace\Scripts";
        const string HexFileName = @"microbit-samples-combined.hex";
        
        public Form1()
        {
            InitializeComponent();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach(DriveInfo i in allDrives)
            {
                SerialPortList.Items.Add(i.RootDirectory);
            }
            SerialPortList.Text = SerialPortList.Items[0].ToString();
            
            YottaHexPath.Text = BuildHexPath;
            HexPath.Text = DefultPath + BuildHexPath;
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            YottaHexPath.Text = path.SelectedPath;
            HexPath.Text = Path.Combine(path.SelectedPath , BuildHexPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.Copy(HexPath.Text, Path.Combine(SerialPortList.Text, HexFileName), true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            HexPath.Text = path.SelectedPath;
            
        }

        private void DriveUpdata_Click(object sender, EventArgs e)
        {
            SerialPortList.Items.Clear();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo i in allDrives)
            {
                SerialPortList.Items.Add(i.RootDirectory);
            }
            SerialPortList.Text = SerialPortList.Items[0].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Process p = new Process();
            

            p.StartInfo.FileName = "cmd.exe";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true; //不跳出cmd視窗
            
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("set PATH=%YOTTA_PATH%;%PATH%");
            p.StandardInput.WriteLine("cmd /K \"cd % HOMEPATH % & C:\\yotta\\workspace\\Scripts\\activate\"");
            p.StandardInput.WriteLine("cd " + DefultPath);
            p.StandardInput.WriteLine("yt build");
            p.StandardInput.WriteLine("exit");
            p.StandardInput.WriteLine("exit");

            StreamReader reader = p.StandardOutput;//截取输出流
            string line = reader.ReadLine();//每次读取一行
            cmdOutput.AppendText(line + "\n");
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                cmdOutput.AppendText(line + "\n");
            }
            
            p.WaitForExit();
            p.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

