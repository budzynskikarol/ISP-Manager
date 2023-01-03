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
    public partial class Klienci : Form
    {
        Regex zip = new Regex(@"^[0-9]{2}\-[0-9]{3}$");
        int row_count = 0;
        bool wszystko_ok;
        string pesel_ok = null;
        string NIP_ok = null;

        public Klienci()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            resetAll();
        }

        private void Klienci_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Imię";
            dataGridView1.Columns[3].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[4].HeaderCell.Value = "Ulica";
            dataGridView1.Columns[5].HeaderCell.Value = "Miasto";
            dataGridView1.Columns[6].HeaderCell.Value = "Kod pocztowy";
            dataGridView1.Columns[7].HeaderCell.Value = "Pesel";
            dataGridView1.Columns[8].HeaderCell.Value = "NIP";
            dataGridView1.Columns[9].HeaderCell.Value = "Telefon";
            dataGridView1.Columns[10].HeaderCell.Value = "Komentarz";
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            textBox7.Text = null;
            textBox8.Text = null;
            textBox9.Text = null;
            textBox10.Text = null;
            textBox11.Text = null;
            comboBox1.Text = null;

            Dodaj.Enabled = true;
            Aktualizuj.Enabled = false;
            Usun.Enabled = false;

            updateDataGrid();

            if (row_count > 0)
            {
                dataGridView1.CurrentRow.Selected = false;
            }
            
            resetErrorLabels();
        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Ulica", System.Data.SqlDbType.NVarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Miasto", System.Data.SqlDbType.NVarChar).Value = textBox6.Text;
                    command.Parameters.Add("@Kod_pocztowy", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@Pesel", System.Data.SqlDbType.NVarChar).Value = textBox8.Text;
                    command.Parameters.Add("@Nip", System.Data.SqlDbType.NVarChar).Value = textBox9.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = textBox10.Text;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox11.Text;
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Ulica", System.Data.SqlDbType.NVarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Miasto", System.Data.SqlDbType.NVarChar).Value = textBox6.Text;
                    command.Parameters.Add("@Kod_pocztowy", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@Pesel", System.Data.SqlDbType.NVarChar).Value = textBox8.Text;
                    command.Parameters.Add("@Nip", System.Data.SqlDbType.NVarChar).Value = textBox9.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.NVarChar).Value = textBox10.Text;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox11.Text;
                    break;
                case 2:
                    msg = "Pomyślnie usunięto użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = 
                        dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    break;
            }

            int n = command.ExecuteNonQuery();
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
                label12.Visible = true;
            }

            if (textBox4.Text == "")
            {
                wszystko_ok = false;
                label13.Visible = true;
            }

            if (textBox5.Text == "")
            {
                wszystko_ok = false;
                label14.Visible = true;
            }

            if (textBox6.Text == "")
            {
                wszystko_ok = false;
                label15.Visible = true;
            }

            if (!zip.IsMatch(textBox7.Text))
            {
                wszystko_ok = false;
                label16.Visible = true;
            }

            if (textBox8.Text.Length < 11)
            {
                wszystko_ok = false;
                label17.Visible = true;
            }

            if (textBox9.Text.Length > 0)
            {
                if (textBox9.Text.Length < 10)
                {
                    wszystko_ok = false;
                    label18.Visible = true;
                }
            }

            if (textBox10.Text.Length < 9)
            {
                wszystko_ok = false;
                label19.Visible = true;
            }
        }

        private void resetErrorLabels()
        {
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
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
                    textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    textBox6.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    textBox7.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    textBox8.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    textBox9.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    textBox10.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                    textBox11.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                }
            }
        }
                 
        private void wyszukiwanie_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Id")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Id_Clients like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Nazwa")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Nazwa like '"
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
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Imie like '"
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
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Nazwisko like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Ulica")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Ulica like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Miasto")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Miasto like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Kod pocztowy")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Kod_pocztowy like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Pesel")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Pesel like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "NIP")
            {
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Nip like '"
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
                SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Telefon like '"
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

        private void OnlyDigits2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
                e.Handled = true;
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
                SqlCommand command = new SqlCommand("SELECT COUNT(Pesel) AS Expr1 FROM Clients WHERE Pesel like '"
                        + textBox8.Text + "'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                
                while (dr.Read())
                {
                    pesel_ok = dr[0].ToString();
                }
                dr.Close();
                
                if (int.Parse(pesel_ok) > 0)
                {
                    wszystko_ok2 = false;
                    MessageBox.Show("Istnieje klient o podanym numerze Pesel!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (textBox9.Text.Length > 0)
                {
                    SqlCommand command2 = new SqlCommand("SELECT COUNT(Nip) AS Expr1 FROM Clients WHERE Nip like '"
                        + textBox9.Text + "'", setConnection.con);
                    SqlDataReader dr2 = command2.ExecuteReader();

                    while (dr2.Read())
                    {
                        NIP_ok = dr2[0].ToString();
                    }
                    dr2.Close();

                    if (int.Parse(NIP_ok) > 0)
                    {
                        wszystko_ok2 = false;
                        MessageBox.Show("Istnieje klient o podanym numerze NIP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            
                if (wszystko_ok2)
                {
                    String sql = "INSERT INTO Clients (Nazwa, Imie, Nazwisko, Ulica, Miasto, Kod_pocztowy, Pesel, Nip, Telefon, Komentarz) " +
                                    "VALUES (@Nazwa, @Imie, @Nazwisko, @Ulica, @Miasto, @Kod_pocztowy, @Pesel, @Nip, @Telefon, @Komentarz)";
                    this.updateDatabase(sql, 0);
                    resetAll();
                }
            }
        }
        
        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[0].Value.ToString() == "1")
            {
                MessageBox.Show("Nie można modyfikować tego klienta", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;

                    if (!(string.Equals(textBox8.Text, dataGridView1.CurrentRow.Cells[7].Value.ToString())))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Pesel) AS Expr1 FROM Clients WHERE Pesel like '"
                            + textBox8.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            pesel_ok = dr[0].ToString();
                        }
                        dr.Close();

                        if (int.Parse(pesel_ok) > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje klient o podanym numerze Pesel!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (wszystko_ok2)
                    {
                        if (textBox9.Text.Length > 0)
                        {
                            if (!(string.Equals(textBox9.Text, dataGridView1.CurrentRow.Cells[8].Value.ToString())))
                            {
                                SqlCommand command2 = new SqlCommand("SELECT COUNT(Nip) AS Expr1 FROM Clients WHERE Nip like '"
                                        + textBox9.Text + "'", setConnection.con);
                                SqlDataReader dr2 = command2.ExecuteReader();

                                while (dr2.Read())
                                {
                                    NIP_ok = dr2[0].ToString();
                                }
                                dr2.Close();

                                if (int.Parse(NIP_ok) > 0)
                                {
                                    wszystko_ok2 = false;
                                    MessageBox.Show("Istnieje klient o podanym numerze NIP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "UPDATE Clients SET Nazwa = @Nazwa, Imie = @Imie, Nazwisko = @Nazwisko, Ulica = @Ulica, " +
                                        "Miasto = @Miasto, Kod_pocztowy = @Kod_pocztowy, Pesel = @Pesel, Nip = @Nip, Telefon = @Telefon, Komentarz = @Komentarz " +
                                        "WHERE Id_Clients = @Id_Clients";
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
                MessageBox.Show("Nie można usunąć tego klienta", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Usunąć klienta: " + textBox3.Text + " "
                    + textBox4.Text + ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                    + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    String sql = "DELETE FROM Clients " +
                    "WHERE Id_Clients = @Id_Clients";
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
    }
}
