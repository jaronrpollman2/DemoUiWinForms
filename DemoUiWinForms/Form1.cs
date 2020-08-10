using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DemoUiWinForms
{
    public partial class Form1 : Form
    {
        static Stack<string> backButton = new Stack<string>();      //Holds previous directories
        static Stack<string> forwardButton = new Stack<string>();   //Holds directories that have been paged back through

        static string path = "";

        DirectoryInfo dirInfo;

        public Form1()
        {
            InitializeComponent();
            path = @"C:\";  //Default starting Path
            txt_URL.Text = path; //Set Default path on startup

            dirInfo = new DirectoryInfo(path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadContents();
        }

        /// <summary>
        /// Loads the files in the currently viewed directory and assign them an icon
        /// </summary>
        private void LoadContents()
        {
            lst_Files.Items.Clear();

            //Try to open file or folder with current permissions
            try
            {
                foreach (var ele in dirInfo.GetFiles())
                {
                    //If an executable or link.
                    if(ele.Extension.Equals(".exe") || ele.Extension.Equals(".lnk"))
                    {
                        lst_Files.Items.Add(ele.Name, 258);
                    }
                    else
                    {
                        lst_Files.Items.Add(ele.Name, 220);
                    }
                }
                foreach (var ele in dirInfo.GetDirectories())
                {
                    lst_Files.Items.Add(ele.Name, 382);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("You do not have permission to access this directory!");

                //Perform stack sanitization so the directory doesn't try to get accessed again
                Back();
                forwardButton.Clear();
                btn_Forward.Enabled = false;
            }
        }

        /// <summary>
        /// Function to handle the action of going one page backwards
        /// </summary>
        private void Back()
        {
            if (backButton.Count > 0)
            {
                //Push current directory on to the Forward Stack
                forwardButton.Push(path);
                btn_Forward.Enabled = true;

                //Update path
                string newPath = backButton.Pop();
                dirInfo = new DirectoryInfo(newPath);
                txt_URL.Text = newPath;

                //Change the old path to the new path
                path = newPath;
                LoadContents();

                //Disable button if nothing is in the stack
                if (backButton.Count <= 0)
                {
                    btn_Back.Enabled = false;
                }
            }
            else
            {
                btn_Back.Enabled = false;
            }

        }

        /// <summary>
        /// Function to handle the action of going one page forward
        /// </summary>
        private void Forward()
        {
            if (forwardButton.Count > 0)
            {
                //Push current directory on the backward stack
                backButton.Push(path);
                btn_Back.Enabled = true;

                //Update path
                string newPath = forwardButton.Pop();
                dirInfo = new DirectoryInfo(newPath);
                txt_URL.Text = newPath;

                //Change the old path to the new path
                path = newPath;
                LoadContents();


                //Disable button if nothing is in the stack
                if (forwardButton.Count <= 0)
                {
                    btn_Forward.Enabled = false;
                }
            }
            else
            {
                btn_Forward.Enabled = false;
            }
        }

        /* Action Listeners for the UI */

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string newPath = "";

            //If File
            if (lst_Files.SelectedItems[0].Index < dirInfo.GetFiles().Length)
            {
                newPath = dirInfo.GetFiles()[lst_Files.SelectedItems[0].Index].FullName;
                FileInfo fInfo = new FileInfo(newPath);

                if(fInfo.Extension.Equals(".exe") || fInfo.Extension.Equals(".lnk")) ///Only want .exe and shortcuts to be opened
                {
                    Process proc = new Process();
                    proc.StartInfo = new ProcessStartInfo {FileName = newPath};
                    proc.Start();
                }
            }
            //Else Directory
            else
            {
                newPath = dirInfo.GetDirectories()[lst_Files.SelectedItems[0].Index - dirInfo.GetFiles().Length].FullName;
                dirInfo = new DirectoryInfo(newPath);
                backButton.Push(path);
                btn_Back.Enabled = true;
                forwardButton.Clear();
                btn_Forward.Enabled = false;
                path = newPath;
                LoadContents();
            }

            txt_URL.Text = newPath;

        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btn_Forward_Click(object sender, EventArgs e)
        {
            Forward();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadContents();
        }

        private void lst_Files_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
                Back();
            if (e.Button == MouseButtons.XButton2)
                Forward();
        }

        private void lst_Files_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter)
            {
                //Alt+Enter to open explorer.exe (Will require password)
                PasswordValidation pwForm = new PasswordValidation();
                pwForm.ShowDialog();
                pwForm.Focus();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Prevents user from closing the form.
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);
        }
    }
}
