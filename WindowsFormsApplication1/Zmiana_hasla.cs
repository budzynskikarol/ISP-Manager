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
    public partial class Zmiana_hasla : Form
    {
        bool wszystko_ok;
         
        public Zmiana_hasla()
        {
            InitializeComponent();
            textBox1.Select();
        }

        private void updateDatabase(String sql_stmt)
        {
            setConnection.Connection();
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);
            command.Parameters.Clear();
            command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = label6.Text;
            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = Hash.Get_Hash(textBox2.Text);

            int n = command.ExecuteNonQuery();
            setConnection.con.Close();
        }

        private void check_boxy()
        {
            wszystko_ok = true;
            resetErrorLabels();

            if (Hash.Get_Hash(textBox1.Text)!= label5.Text)
            {
                wszystko_ok = false;
                label1.Visible = true;
            }
            else if (textBox1.Text == textBox2.Text)
            {
                wszystko_ok = false;
                label2.Visible = true;
            }
            else if (!(isStrongPassword.Password(textBox2.Text)))
            {
                wszystko_ok = false;
                label3.Visible = true;
            }

            if (textBox2.Text != textBox3.Text)
            {
                wszystko_ok = false;
                label4.Visible = true;
            }
        }

        private void resetErrorLabels()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Exit.tak = false;
            check_boxy();
            if (wszystko_ok)
            {
                String sql = "UPDATE Users SET Password = @Password WHERE Login = @Login";
                updateDatabase(sql);
                MessageBox.Show("Pomyślnie zmieniono hasło!\nNastąpi zamknięcie systemu.\nProszę się ponownie zalogować.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Exit.tak = true;
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.UseSystemPasswordChar)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.UseSystemPasswordChar)
            {
                textBox3.UseSystemPasswordChar = false;
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
            }
        }
    }
}
