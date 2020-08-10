using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoUiWinForms
{
    public partial class PasswordValidation : Form
    {
        public PasswordValidation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidatePass();
        }

        private void txt_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ValidatePass();
            }
        }

        private void ValidatePass()
        {
            //Quick and dirty password validation
            string password = "12345";
            if (txt_Password.Text == password)
            {
                ProcessStartInfo explorerStartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\explorer.exe"

                };
                Process explorer = new Process();
                explorer.StartInfo = explorerStartInfo;

                explorer.Start();
                Thread.Sleep(1000);
                this.Close();
            }
        }
    }
}
