using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ISP_Manager
{
    public partial class Login : Form
    {
        SqlDataReader rdr;
        bool Klienci_check, Umowy_check, Abonamenty_check, Wplaty_check, Zlecenia_check, Urzadzenia_check, Pracownicy_check;
        string err2 = null;

        public Login()
        {
            InitializeComponent();
            TLogin.Select();
        }

        private void updateDatabase(String sql_stmt)
        {
            setConnection.Connection();
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);
            command.Parameters.Clear();
            command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = TLogin.Text;
            command.Parameters.Add("@Logowanie", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            int n = command.ExecuteNonQuery();
            setConnection.con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass = null;

            using (SqlConnection connection = new SqlConnection(setConnection.conn_str))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException err)
                {
                    err2 = err.ToString();
                    MessageBox.Show("Błąd połączenia z bazą danych", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("SELECT Password, Klienci, Umowy, Abonamenty, Wplaty, " +
                    "Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Login=@Login", connection);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = TLogin.Text;
                    rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        pass = rdr[0].ToString();
                        Klienci_check = (bool)rdr[1];
                        Umowy_check = (bool)rdr[2];
                        Abonamenty_check = (bool)rdr[3];
                        Wplaty_check = (bool)rdr[4];
                        Zlecenia_check = (bool)rdr[5];
                        Urzadzenia_check = (bool)rdr[6];
                        Pracownicy_check = (bool)rdr[7];
                    }

                    if (rdr != null) rdr.Close();
                    connection.Close();
                }
            }

            if ((err2 == null) && (pass != ""))
            {
                if (Hash.Get_Hash(TPass.Text) == pass)
                {
                    String sql = "UPDATE Users SET Logowanie = @Logowanie WHERE Login = @Login";
                    updateDatabase(sql);
                    Glowne Glowne = new Glowne();
                    if (!Klienci_check)
                        Glowne.Klienci.Enabled = false;
                    if (!Umowy_check)
                        Glowne.Umowy.Enabled = false;
                    if (!Abonamenty_check)
                        Glowne.Abonamenty.Enabled = false;
                    if (!Wplaty_check)
                        Glowne.Finanse.Enabled = false; 
                    if (!Zlecenia_check)
                        Glowne.Zlecenia.Enabled = false;
                    if (!Urzadzenia_check)
                        Glowne.Urzadzenia.Enabled = false;
                    if (!Pracownicy_check)
                        Glowne.Pracownicy.Enabled = false;
                    Glowne.toolStripStatusLabel1.Text = TLogin.Text;
                    Glowne.toolStripStatusLabel3.Text = pass;
                    Glowne.ShowDialog();
                    Application.ExitThread();
                }
                else
                    MessageBox.Show("Niepoprawny login lub hasło!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (TPass.UseSystemPasswordChar)
            {
                TPass.UseSystemPasswordChar = false;
            }
            else
            {
                TPass.UseSystemPasswordChar = true;
            }
        }
    }
}
