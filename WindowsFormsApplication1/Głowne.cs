using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using ISP_Manager;

namespace ISP_Manager
{
    public partial class Głowne : Form
    {      
        public Głowne()
        {
            InitializeComponent();
        }
        
        private void Klienci_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Form22")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Klienci Admin = new Klienci();
                Admin.MdiParent = this;
                Admin.Show();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Form22")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Form22 Admin = new Form22();
                //Admin.label1.Text = "LALALA";
                //Admin.label28.Text = "LALALA";
                //Admin.label29.Text = "LALALA";
                //Admin.label30.Text = "LALALA";
                //Admin.label26.Text = "LALALA";
                //Admin.label37.Text = pass;
                Admin.MdiParent = this;
                Admin.Show();
            }
        }
    }
}
