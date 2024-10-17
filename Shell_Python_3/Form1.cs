using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Net;
using System.Threading;
namespace Shell_Python_3
{
    public partial class Form1 : Form
    {
        Instructions ins = new Instructions();


        public Form1()
        {
            InitializeComponent();
            ins.start_cmd();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2
            {
                Left = this.Left,
                Top = this.Top
            };

            frm.Show();
            this.WindowState = FormWindowState.Minimized;


        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //обрабатываем щелчок левой
            if (e.Button == MouseButtons.Left)
            {
                ins.process_Python.CloseMainWindow();
                Thread.Sleep(100);
                Application.Exit();
            }
            else
            //обрабатываем щелчок правой
            if (e.Button == MouseButtons.Right)
            {
                this.Left = Cursor.Position.X - this.Size.Width;
                this.Top = Cursor.Position.Y - this.Size.Height;

                this.TopMost = true;
                Instructions.ShowWindow(this.Handle, 5);
                this.WindowState = FormWindowState.Normal;

                label1.Text = "ProcessName - " + ins.process_Python.ProcessName;
                label2.Text = "MainWindowTitle - " + ins.process_Python.MainWindowTitle;
                label3.Text = "PID - " + ins.process_Python.Id;
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Instructions.ShowWindow(this.Handle, 0);
            this.WindowState = FormWindowState.Minimized;
        }
    }

    public class Instructions
    {
        public Process process_Python = new Process();
        public Thread thread;

        public void start_cmd()
        {
            if (settings())
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = @"/C start " + ConfigurationManager.ConnectionStrings["Path"].ConnectionString + " ^& " + ConfigurationManager.ConnectionStrings["Environment"].ConnectionString;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process.Start();

                thread = new Thread(Find_Proc_Python);
                thread.Start();
            }
        }

        public bool settings()
        {
            if (!File.Exists(ConfigurationManager.ConnectionStrings["Path"].ConnectionString))
            {
                Form2 frm = new Form2();
                frm.Show();
                return false;
            }
            return true;
        }

        public void Find_Proc_Python()
        {
            bool flag = true;
            while (flag)
            {
                Thread.Sleep(100);
                var processes = Process.GetProcessesByName("cmd");

                if (processes.Length != 0)
                    foreach (var pr in processes)
                        if (pr.MainWindowTitle == "npm prefix")
                        {
                            pr.Refresh();
                            process_Python = pr;
                            ShowWindow(pr.MainWindowHandle, 0);
                            flag = false;
                            break;
                        }
            }
        }


        [DllImport("user32.dll")]
        static public extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
