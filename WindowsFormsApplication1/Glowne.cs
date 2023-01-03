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

namespace ISP_Manager
{
    public partial class Glowne : Form
    {
        public Glowne()
        {
            InitializeComponent();
        }

        private void Glowne_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = (1 * 1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            Godzina.Text = DateTime.Now.ToLongTimeString();
            Data.Text = "| " + DateTime.Now.ToLongDateString() + " |";
        }

        private void Glowne_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Exit.tak)
            {
                DialogResult dialogResult = MessageBox.Show("Zamknąć aplikację?",
                "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Glowne_FormClosed(object sender, FormClosedEventArgs e)
        {
            String sql = "UPDATE Users SET Wylogowanie = @Wylogowanie WHERE Login = @Login";
            updateDatabase(sql);
        }

        private void updateDatabase(String sql_stmt)
        {
            setConnection.Connection();
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);
            command.Parameters.Clear();
            command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = toolStripStatusLabel1.Text;
            command.Parameters.Add("@Wylogowanie", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            int n = command.ExecuteNonQuery();
            setConnection.con.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Godzina.Text = DateTime.Now.ToLongTimeString();
        }

        private void Klienci_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Klienci Klienci = new Klienci();
                Klienci.MdiParent = this;
                Klienci.Show();
            }
        }

        private void Umowy_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Umowy Umowy = new Umowy();
                Umowy.MdiParent = this;
                Umowy.Show();
            }
        }

        private void Abonamenty_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Abonamenty Abonamenty = new Abonamenty();
                Abonamenty.MdiParent = this;
                Abonamenty.Show();
            }
        }

        private void Finanse_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Finanse Finanse = new Finanse();
                Finanse.MdiParent = this;
                Finanse.Show();
            }

        }

        private void Zlecenia_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Zlecenia Zlecenia = new Zlecenia();
                Zlecenia.MdiParent = this;
                Zlecenia.Show();
            }
        }

        private void Urzadzenia_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Urzadzenia Urzadzenia = new Urzadzenia();
                Urzadzenia.MdiParent = this;
                Urzadzenia.Show();
            }

        }

        private void Pracownicy_Click(object sender, EventArgs e)
        {
            bool isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Klienci" || f.Text == "Umowy" || f.Text == "Abonamenty" || f.Text == "Finanse"
                    || f.Text == "Zlecenia" || f.Text == "Urządzenia" || f.Text == "Pracownicy")
                {
                    isopen = true;
                    MessageBox.Show("Zamknij otwarte okno!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }

            if (isopen == false)
            {
                Pracownicy Pracownicy = new Pracownicy();
                Pracownicy.MdiParent = this;
                Pracownicy.Show();
            }
        }

        private void Zmiana_hasla_Click(object sender, EventArgs e)
        {
            Zmiana_hasla Zmiana_hasla = new Zmiana_hasla();
            Zmiana_hasla.label5.Text = toolStripStatusLabel3.Text;
            Zmiana_hasla.label6.Text = toolStripStatusLabel1.Text;
            Zmiana_hasla.ShowDialog();
        }

        private void Koniec_Click(object sender, EventArgs e)
        {
            Exit.tak = false;
            DialogResult dialogResult = MessageBox.Show("Zamknąć aplikację?",
                "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Exit.tak = true;
                Application.Exit();
            }
        }
    }
}
