using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shell_Python_3
{
    public partial class Form2 : Form
    {
        Instructions ins = new Instructions();

        public Form2()
        {
            InitializeComponent();

            textBox1.Text = ConfigurationManager.ConnectionStrings["Path"].ConnectionString;
            comboBox1.Text = ConfigurationManager.ConnectionStrings["Environment"].ConnectionString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["Path"].ConnectionString = textBox1.Text;
            config.Save();

            connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["Environment"].ConnectionString = comboBox1.Text;
            config.Save();

            ConfigurationManager.RefreshSection("connectionStrings");

            Application.Restart();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
