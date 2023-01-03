using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ISP_Manager
{
    public partial class Pracownicy : Form
    {
        int row_count = 0;
        bool wszystko_ok;
        string login_ok = null;
        string old_pass = null;

        public Pracownicy()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
        }

        private void Uzytkownicy_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            resetAll();
        }

        private void Uzytkownicy_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Login";
            dataGridView1.Columns[2].HeaderCell.Value = "Imię";
            dataGridView1.Columns[3].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[4].HeaderCell.Value = "Telefon";
            dataGridView1.Columns[5].HeaderCell.Value = "Logowanie";
            dataGridView1.Columns[6].HeaderCell.Value = "Wylogowanie";
            dataGridView1.Columns[7].HeaderCell.Value = "Klienci";
            dataGridView1.Columns[8].HeaderCell.Value = "Umowy";
            dataGridView1.Columns[9].HeaderCell.Value = "Abonamenty";
            dataGridView1.Columns[10].HeaderCell.Value = "Wpłaty";
            dataGridView1.Columns[11].HeaderCell.Value = "Zlecenia";
            dataGridView1.Columns[12].HeaderCell.Value = "Urządzenia";
            dataGridView1.Columns[13].HeaderCell.Value = "Pracownicy";

        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano pracownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = Hash.Get_Hash(textBox12.Text);
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = textBox10.Text;
                    command.Parameters.Add("@Klienci", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox1.Checked.ToString());
                    command.Parameters.Add("@Umowy", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox2.Checked.ToString());
                    command.Parameters.Add("@Abonamenty", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox3.Checked.ToString());
                    command.Parameters.Add("@Wplaty", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox4.Checked.ToString());
                    command.Parameters.Add("@Zlecenia", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox5.Checked.ToString());
                    command.Parameters.Add("@Urzadzenia", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox6.Checked.ToString());
                    command.Parameters.Add("@Pracownicy", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox7.Checked.ToString());
                    break;
                case 1:
                    if (checkBox8.Checked)
                    {
                        old_pass = Hash.Get_Hash(textBox12.Text);
                    }
                    msg = "Pomyślnie zaktualizowano pracownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Users", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    command.Parameters.Add("@Login", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = old_pass;
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = textBox10.Text;
                    command.Parameters.Add("@Klienci", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox1.Checked.ToString());
                    command.Parameters.Add("@Umowy", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox2.Checked.ToString());
                    command.Parameters.Add("@Abonamenty", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox3.Checked.ToString());
                    command.Parameters.Add("@Wplaty", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox4.Checked.ToString());
                    command.Parameters.Add("@Zlecenia", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox5.Checked.ToString());
                    command.Parameters.Add("@Urzadzenia", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox6.Checked.ToString());
                    command.Parameters.Add("@Pracownicy", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox7.Checked.ToString());
                    break;
                case 2:
                    msg = "Pomyślnie usunięto pracownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Users", System.Data.SqlDbType.Int).Value =
                        dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    break;
                case 3:
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Users", System.Data.SqlDbType.Int).Value =
                        dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    break;
            }

            int n = command.ExecuteNonQuery();
            if (state!=3)
            if (n > 0)
            {
                MessageBox.Show(msg, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateDataGrid();
            }
        }

        private void check_boxy()
        {
            wszystko_ok = true;
            resetErrorLabels();

            if (textBox3.Text == "")
            {
                wszystko_ok = false;
                label21.Visible = true;
            }

            if (Dodaj.Enabled || checkBox8.Checked)
            {
                if (!(isStrongPassword.Password(textBox12.Text)))
                {
                    wszystko_ok = false;
                    label22.Visible = true;
                }
            }

            if (textBox3.Text == "")
            {
                wszystko_ok = false;
                label12.Visible = true;
            }

            if (textBox4.Text == "")
            {
                wszystko_ok = false;
                label13.Visible = true;
            }

            if (textBox10.Text.Length < 9)
            {
                wszystko_ok = false;
                label19.Visible = true;
            }
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox10.Text = null;
            textBox12.Text = null;
            comboBox1.Text = null;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;

            Dodaj.Enabled = true;
            Aktualizuj.Enabled = false;
            Usun.Enabled = false;
            checkBox8.Visible = false;

            updateDataGrid();

            if (row_count > 0)
            {
                dataGridView1.CurrentRow.Selected = false;
            }

            resetErrorLabels();
        }

        private void resetErrorLabels()
        {
            label12.Visible = false;
            label13.Visible = false;
            label19.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (row_count > 0)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    textBox10.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    checkBox1.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[7].Value.ToString());
                    checkBox2.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[8].Value.ToString());
                    checkBox3.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[9].Value.ToString());
                    checkBox4.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[10].Value.ToString());
                    checkBox5.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[11].Value.ToString());
                    checkBox6.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[12].Value.ToString());
                    checkBox7.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[13].Value.ToString());

                    SqlCommand command = new SqlCommand("SELECT Password FROM Users WHERE Id_Users like '"
                        + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        old_pass = dr[0].ToString();
                    }
                    dr.Close();

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                    checkBox8.Visible = true;
                    checkBox8.Checked = false;
                    textBox12.Text = "";
                    resetErrorLabels();
                }
            }
        }

        private void wyszukiwanie_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Id")
            {
                SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Id_Users like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Login")
            {
                SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Login like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Imię")
            {
                SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Imie like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Nazwisko")
            {
                SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Nazwisko like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }

            else if (comboBox1.Text == "Telefon")
            {
                SqlCommand command = new SqlCommand("SELECT Id_Users, Login, Imie, Nazwisko, Telefon, Logowanie, " +
                "Wylogowanie, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy FROM Users WHERE Telefon like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
        }

        private void OnlyDigits_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = null;
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                bool wszystko_ok2 = true;
                SqlCommand command = new SqlCommand("SELECT COUNT(Login) AS Expr1 FROM Users WHERE Login like '"
                        + textBox2.Text + "'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    login_ok = dr[0].ToString();
                }
                dr.Close();

                if (int.Parse(login_ok) > 0)
                {
                    wszystko_ok2 = false;
                    MessageBox.Show("Istnieje pracownik o podanym Loginie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (wszystko_ok2)
                {
                    String sql = "INSERT INTO Users (Login, Password, Imie, Nazwisko, Telefon, Klienci, Umowy, Abonamenty, Wplaty, Zlecenia, Urzadzenia, Pracownicy) " +
                                    "VALUES (@Login, @Password, @Imie, @Nazwisko, @Telefon, @Klienci, @Umowy, @Abonamenty, @Wplaty, @Zlecenia, @Urzadzenia, @Pracownicy)";
                    this.updateDatabase(sql, 0);
                    resetAll();
                }
            }
        }

        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "1")
            {
                MessageBox.Show("Nie można modyfikować tego użytkownika", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;

                    if (!(string.Equals(textBox2.Text, dataGridView1.CurrentRow.Cells[1].Value.ToString())))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Login) AS Expr1 FROM Users WHERE Login like '"
                            + textBox2.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            login_ok = dr[0].ToString();
                        }
                        dr.Close();

                        if (int.Parse(login_ok) > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje pracownik o podanym Loginie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "UPDATE Users SET Login = @Login, Password = @Password, Imie = @Imie, Nazwisko = @Nazwisko, " +
                                        "Telefon = @Telefon, Klienci = @Klienci, Umowy = @Umowy, Abonamenty = @Abonamenty,  " +
                                        "Wplaty = @Wplaty, Zlecenia = @Zlecenia, Urzadzenia = @Urzadzenia, Pracownicy = @Pracownicy " +
                                        "WHERE Id_Users = @Id_Users";
                        this.updateDatabase(sql, 1);
                        resetAll();
                    }
                }
            }
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "1")
            {
                MessageBox.Show("Nie można usunąć tego użytkownika", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Usunąć pracownika: " + textBox3.Text + " "
                    + textBox4.Text + ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                    + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string sql = "UPDATE Orders SET Id_Users = '1' WHERE Id_Users = @Id_Users";
                    updateDatabase(sql, 3);
                    sql = "DELETE FROM Users " +
                    "WHERE Id_Users = @Id_Users";
                    updateDatabase(sql, 2);
                    resetAll();
                }
            }
        }

        private void Wyczysc_Click(object sender, EventArgs e)
        {
            resetAll();
        }

        private void Zamknij_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox12.UseSystemPasswordChar)
            {
                textBox12.UseSystemPasswordChar = false;
            }
            else
            {
                textBox12.UseSystemPasswordChar = true;
            }
        }
    }
}
