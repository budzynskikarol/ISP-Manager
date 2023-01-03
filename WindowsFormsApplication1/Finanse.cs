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
    public partial class Finanse : Form
    {
        Regex kwota = new Regex(@"^[0-9]+\,[0-9]{2}$");
        int row_count = 0;
        bool wszystko_ok, nalicz, saldo;
        string[] id_klient;
        string typ = null;
        string rok = null;
        string komenda_finanse = "SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, Finances.Typ, " +
                "Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data " +
                "FROM Clients INNER JOIN Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE " +
                "(Clients.Id_Clients = @Id_Clients) AND (Finances.Data BETWEEN @Data_p AND @Data_k)";

        public Finanse()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
        }

        private void Finanse_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            fillcombo();
            resetAll();
        }

        private void Finanse_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Kwota";
            dataGridView1.Columns[3].HeaderCell.Value = "Typ";
            dataGridView1.Columns[4].HeaderCell.Value = "Id klienta";
            dataGridView1.Columns[5].HeaderCell.Value = "Imię";
            dataGridView1.Columns[6].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[7].HeaderCell.Value = "Data";
        }

        private void resetAll()
        {
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            styczen_n.Text = null;
            styczen_w.Text = null;
            styczen_s.Text = null;
            luty_n.Text = null;
            luty_w.Text = null;
            luty_s.Text = null;
            marzec_n.Text = null;
            marzec_w.Text = null;
            marzec_s.Text = null;
            kwiecien_n.Text = null;
            kwiecien_w.Text = null;
            kwiecien_s.Text = null;
            maj_n.Text = null;
            maj_w.Text = null;
            maj_s.Text = null;
            czerwiec_n.Text = null;
            czerwiec_w.Text = null;
            czerwiec_s.Text = null;
            lipiec_n.Text = null;
            lipiec_w.Text = null;
            lipiec_s.Text = null;
            sierpien_n.Text = null;
            sierpien_w.Text = null;
            sierpien_s.Text = null;
            wrzesien_n.Text = null;
            wrzesien_w.Text = null;
            wrzesien_s.Text = null;
            pazdziernik_n.Text = null;
            pazdziernik_w.Text = null;
            pazdziernik_s.Text = null;
            listopad_n.Text = null;
            listopad_w.Text = null;
            listopad_s.Text = null;
            grudzien_n.Text = null;
            grudzien_w.Text = null;
            grudzien_s.Text = null;
            pods_n.Text = null;
            pods_w.Text = null;
            pods_s.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            dateTimePicker1.ResetText();
            dateTimePicker2.ResetText();

            Dodaj.Enabled = true;
            Aktualizuj.Enabled = false;
            Usun.Enabled = false;

            Styczen.Enabled = false;
            Luty.Enabled = false;
            Marzec.Enabled = false;
            Kwiecien.Enabled = false;
            Maj.Enabled = false;
            Czerwiec.Enabled = false;
            Lipiec.Enabled = false;
            Sierpien.Enabled = false;
            Wrzesien.Enabled = false;
            Pazdziernik.Enabled = false;
            Listopad.Enabled = false;
            Grudzien.Enabled = false;
            Podsumowanie.Enabled = false;
            
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
                    msg = "Pomyślnie dodano operację finansową!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Kwota", System.Data.SqlDbType.Decimal).Value = Decimal.Parse(textBox4.Text);
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = typ;
                    command.Parameters.Add("@Data", System.Data.SqlDbType.Date).Value = dateTimePicker1.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = Int16.Parse(id_klient[0]);
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano operację finansową!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Finances", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Kwota", System.Data.SqlDbType.Decimal).Value = Decimal.Parse(textBox4.Text);
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = typ;
                    command.Parameters.Add("@Data", System.Data.SqlDbType.Date).Value = dateTimePicker1.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = Int16.Parse(id_klient[0]);
                    break;
                case 2:
                    msg = "Pomyślnie usunięto operację finansową!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Finances", System.Data.SqlDbType.Int).Value =
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

            if (!nalicz && !saldo)
            {
                if (textBox2.Text == "")
                {
                    wszystko_ok = false;
                    label4.Visible = true;
                }

                if (!kwota.IsMatch(textBox4.Text))
                {
                    wszystko_ok = false;
                    label13.Visible = true;
                }

                if (!checkBox1.Checked && !checkBox2.Checked)
                {
                    wszystko_ok = false;
                    label6.Visible = true;
                }

                if (comboBox2.Text == "")
                {
                    wszystko_ok = false;
                    label17.Visible = true;
                }
            }
            else if (nalicz)
            {
                nalicz = false;

                if (textBox5.Text == "")
                {
                    wszystko_ok = false;
                    label9.Visible = true;
                }
            }
            else if (saldo)
            {
                saldo = false;

                if (comboBox1.Text == "")
                {
                    wszystko_ok = false;
                    label8.Visible = true;
                }

                if (comboBox4.Text == "")
                {
                    wszystko_ok = false;
                    label15.Visible = true;
                }
            }

        }

        private void resetErrorLabels()
        {
            label4.Visible = false;
            label6.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label13.Visible = false;
            label15.Visible = false;
            label17.Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (row_count > 0)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == "Wpłata")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = true;
                    }
                    comboBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[5].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                }
            }
        }

        private void fillcombo()
        {
            SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Id_Clients NOT LIKE 1", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string sName = dr.GetInt32(0) + " | " + dr.GetString(2) + " | " + dr.GetString(3);
                comboBox1.Items.Add(sName);
                comboBox2.Items.Add(sName);
            }
            dr.Close();
        }

        private void wyszukiwanie_TextChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "Id operacji")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Id_Finances like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Nazwa")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Nazwa like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Kwota")
            {
                if (textBox3.Text != "")
                {
                    SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                    "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                    "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Kwota like '"
                        + textBox3.Text + "%'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dataGridView1.DataSource = dt.DefaultView;
                    dr.Close();
                    row_count = dataGridView1.RowCount;
                }
                else
                {
                    updateDataGrid();
                }
            }
            else if (comboBox3.Text == "Typ")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Typ like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Id klienta")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Id_Clients like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Imię")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Clients.Imie like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Nazwisko")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Clients.Nazwisko like '"
                    + textBox3.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox3.Text == "Data")
            {
                SqlCommand command = new SqlCommand("SELECT Finances.Id_Finances, Finances.Nazwa, Finances.Kwota, " +
                "Finances.Typ, Finances.Id_Clients, Clients.Imie, Clients.Nazwisko, Finances.Data FROM Clients INNER JOIN " +
                "Finances ON Clients.Id_Clients = Finances.Id_Clients WHERE Finances.Data like '"
                    + textBox3.Text + "%'", setConnection.con);
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
            if (Char.IsLetter(e.KeyChar))
                e.Handled = true;
        }

        private void checkBox1_CheckedStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            styczen_n.Text = null;
            styczen_w.Text = null;
            styczen_s.Text = null;
            luty_n.Text = null;
            luty_w.Text = null;
            luty_s.Text = null;
            marzec_n.Text = null;
            marzec_w.Text = null;
            marzec_s.Text = null;
            kwiecien_n.Text = null;
            kwiecien_w.Text = null;
            kwiecien_s.Text = null;
            maj_n.Text = null;
            maj_w.Text = null;
            maj_s.Text = null;
            czerwiec_n.Text = null;
            czerwiec_w.Text = null;
            czerwiec_s.Text = null;
            lipiec_n.Text = null;
            lipiec_w.Text = null;
            lipiec_s.Text = null;
            sierpien_n.Text = null;
            sierpien_w.Text = null;
            sierpien_s.Text = null;
            wrzesien_n.Text = null;
            wrzesien_w.Text = null;
            wrzesien_s.Text = null;
            pazdziernik_n.Text = null;
            pazdziernik_w.Text = null;
            pazdziernik_s.Text = null;
            listopad_n.Text = null;
            listopad_w.Text = null;
            listopad_s.Text = null;
            grudzien_n.Text = null;
            grudzien_w.Text = null;
            grudzien_s.Text = null;
            pods_n.Text = null;
            pods_w.Text = null;
            pods_s.Text = null;
            comboBox4.Text = null;

            Styczen.Enabled = false;
            Luty.Enabled = false;
            Marzec.Enabled = false;
            Kwiecien.Enabled = false;
            Maj.Enabled = false;
            Czerwiec.Enabled = false;
            Lipiec.Enabled = false;
            Sierpien.Enabled = false;
            Wrzesien.Enabled = false;
            Pazdziernik.Enabled = false;
            Listopad.Enabled = false;
            Grudzien.Enabled = false;
            Podsumowanie.Enabled = false;
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                String sql = "INSERT INTO Finances (Nazwa, Kwota, Typ, Data, Id_Clients) " +
                    "VALUES (@Nazwa, @Kwota, @Typ, @Data, @Id_Clients)";
                if (checkBox1.Checked)
                {
                    typ = "Wpłata";
                }
                else
                {
                    typ = "Naliczenie";
                }
                id_klient = comboBox2.Text.Split('|');
                this.updateDatabase(sql, 0);
                resetAll();
            }
        }

        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                String sql = "UPDATE Finances SET Nazwa = @Nazwa, Kwota = @Kwota, Typ = @Typ, " +
                    "Data = @Data, Id_Clients = @Id_Clients WHERE Id_Finances = @Id_Finances";
                if (checkBox1.Checked)
                {
                    typ = "Wpłata";
                }
                else
                {
                    typ = "Naliczenie";
                }
                id_klient = comboBox2.Text.Split('|');
                this.updateDatabase(sql, 1);
                resetAll();
            }
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Usunąć operację finansową: " + textBox2.Text +
                ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                String sql = "DELETE FROM Finances " +
                "WHERE Id_Finances = @Id_Finances";
                updateDatabase(sql, 2);
                resetAll();
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

        private void Saldo_Click(object sender, EventArgs e)
        {
            saldo = true;
            check_boxy();
            decimal naliczenie, wplata, suma, podsumowanie_n, podsumowanie_w;
            if (wszystko_ok)
            {
                rok = comboBox4.Text;
                id_klient = comboBox1.Text.Split('|');
                podsumowanie_n = 0;
                podsumowanie_w = 0;

                //STYCZEŃ
                suma = 0;
                SqlCommand command = new SqlCommand("SELECT Kwota FROM Finances WHERE (Id_Clients = @Id_Clients) " +
                  "AND (Typ = @Typ) AND (Data BETWEEN @Data_p AND @Data_k)", setConnection.con);
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-01-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-01-31";
                SqlDataReader dr = command.ExecuteReader();

                int a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    styczen_n.Text = naliczenie.ToString();
                }
                else
                {
                    styczen_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-01-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-01-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    styczen_w.Text = wplata.ToString();
                }
                else
                {
                    styczen_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                styczen_s.Text = suma.ToString();

                //LUTY
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-02-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-02-28";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    luty_n.Text = naliczenie.ToString();
                }
                else
                {
                    luty_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-02-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-02-28";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    luty_w.Text = wplata.ToString();
                }
                else
                {
                    luty_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                luty_s.Text = suma.ToString();

                //MARZEC
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-03-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-03-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    marzec_n.Text = naliczenie.ToString();
                }
                else
                {
                    marzec_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-03-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-03-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    marzec_w.Text = wplata.ToString();
                }
                else
                {
                    marzec_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                marzec_s.Text = suma.ToString();

                //KWIECIEŃ
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-04-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-04-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    kwiecien_n.Text = naliczenie.ToString();
                }
                else
                {
                    kwiecien_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-04-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-04-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    kwiecien_w.Text = wplata.ToString();
                }
                else
                {
                    kwiecien_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                kwiecien_s.Text = suma.ToString();

                //MAJ
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-05-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-05-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    maj_n.Text = naliczenie.ToString();
                }
                else
                {
                    maj_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-05-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-05-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    maj_w.Text = wplata.ToString();
                }
                else
                {
                    maj_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                maj_s.Text = suma.ToString();

                //CZERWIEC
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-06-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-06-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    czerwiec_n.Text = naliczenie.ToString();
                }
                else
                {
                    czerwiec_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-06-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-06-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    czerwiec_w.Text = wplata.ToString();
                }
                else
                {
                    czerwiec_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                czerwiec_s.Text = suma.ToString();

                //LIPIEC
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-07-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-07-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    lipiec_n.Text = naliczenie.ToString();
                }
                else
                {
                    lipiec_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-07-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-07-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    lipiec_w.Text = wplata.ToString();
                }
                else
                {
                    lipiec_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                lipiec_s.Text = suma.ToString();

                //SIERPIEŃ
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-08-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-08-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    sierpien_n.Text = naliczenie.ToString();
                }
                else
                {
                    sierpien_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-08-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-08-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    sierpien_w.Text = wplata.ToString();
                }
                else
                {
                    sierpien_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                sierpien_s.Text = suma.ToString();

                //WRZESIEŃ
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-09-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-09-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    wrzesien_n.Text = naliczenie.ToString();
                }
                else
                {
                    wrzesien_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-09-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-09-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    wrzesien_w.Text = wplata.ToString();
                }
                else
                {
                    wrzesien_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                wrzesien_s.Text = suma.ToString();

                //PAŹDZIERNIK
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-10-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-10-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    pazdziernik_n.Text = naliczenie.ToString();
                }
                else
                {
                    pazdziernik_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-10-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-10-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    pazdziernik_w.Text = wplata.ToString();
                }
                else
                {
                    pazdziernik_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                pazdziernik_s.Text = suma.ToString();

                //LISTOPAD
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-11-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-11-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    listopad_n.Text = naliczenie.ToString();
                }
                else
                {
                    listopad_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-11-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-11-30";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    listopad_w.Text = wplata.ToString();
                }
                else
                {
                    listopad_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                listopad_s.Text = suma.ToString();

                //GRUDZIEŃ
                suma = 0;
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-12-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-12-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                naliczenie = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        naliczenie = naliczenie + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    grudzien_n.Text = naliczenie.ToString();
                }
                else
                {
                    grudzien_n.Text = "0,00";
                }

                command.Parameters.Clear();
                command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient[0];
                command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Wpłata";
                command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-12-01";
                command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = comboBox4.Text + "-12-31";
                dr = command.ExecuteReader();

                a = dr.FieldCount;
                wplata = 0;
                if (a > 0)
                {
                    while (dr.Read())
                    {
                        wplata = wplata + decimal.Parse(dr[0].ToString());
                    }
                    dr.Close();
                    grudzien_w.Text = wplata.ToString();
                }
                else
                {
                    grudzien_w.Text = "0,00";
                }

                podsumowanie_n = podsumowanie_n + naliczenie;
                podsumowanie_w = podsumowanie_w + wplata;
                suma = wplata - naliczenie;
                grudzien_s.Text = suma.ToString();

                suma = podsumowanie_w - podsumowanie_n;
                pods_n.Text = podsumowanie_n.ToString();
                pods_w.Text = podsumowanie_w.ToString();
                pods_s.Text = suma.ToString();

                Styczen.Enabled = true;
                Luty.Enabled = true;
                Marzec.Enabled = true;
                Kwiecien.Enabled = true;
                Maj.Enabled = true;
                Czerwiec.Enabled = true;
                Lipiec.Enabled = true;
                Sierpien.Enabled = true;
                Wrzesien.Enabled = true;
                Pazdziernik.Enabled = true;
                Listopad.Enabled = true;
                Grudzien.Enabled = true;
                Podsumowanie.Enabled = true;
            }
        }

        private void Nalicz_Click(object sender, EventArgs e)
        {
            nalicz = true;
            check_boxy();
            if (wszystko_ok)
            {
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Clients, Subscriptions.Cena FROM Contracts " +
                    "INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = Subscriptions.Id_Subscriptions " +
                    "WHERE (Contracts.Obowiazujaca = @Obowiazujaca)", setConnection.con);
                command.Parameters.Clear();
                command.Parameters.Add("@Obowiazujaca", System.Data.SqlDbType.Bit).Value = 1;
                SqlDataReader dr = command.ExecuteReader();
                
                DataTable dt = new DataTable();
                dt.Load(dr);
                int numRows = dt.Rows.Count;
                int[] id_klient2 = new int[numRows];
                decimal[] kwota2 = new decimal[numRows];
                int a = 0;
                
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    id_klient2[a] = Int16.Parse(dr[0].ToString());
                    kwota2[a] = Decimal.Parse(dr[1].ToString());
                    a++;
                }
                dr.Close();

                for (int i = 0; i < a; i++)
                {
                    command = new SqlCommand("INSERT INTO Finances (Nazwa, Kwota, Typ, Data, Id_Clients) " +
                    "VALUES (@Nazwa, @Kwota, @Typ, @Data, @Id_Clients)", setConnection.con);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Kwota", System.Data.SqlDbType.Decimal).Value = kwota2[i];
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = "Naliczenie";
                    command.Parameters.Add("@Data", System.Data.SqlDbType.Date).Value = dateTimePicker2.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient2[i];

                    int result = command.ExecuteNonQuery();
                }

                MessageBox.Show("Ilość naliczonych abonamentów: " + a, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                resetAll();
            }
        }

        private void Styczen_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-01-01";
            Saldo_Okno.data_k_saldo = rok + "-01-31";
            Saldo_Okno.ShowDialog();
        }

        private void Luty_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-02-01";
            Saldo_Okno.data_k_saldo = rok + "-02-28";
            Saldo_Okno.ShowDialog();
        }

        private void Marzec_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-03-01";
            Saldo_Okno.data_k_saldo = rok + "-03-31";
            Saldo_Okno.ShowDialog();
        }

        private void Kwiecien_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-04-01";
            Saldo_Okno.data_k_saldo = rok + "-04-30";
            Saldo_Okno.ShowDialog();
        }

        private void Maj_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-05-01";
            Saldo_Okno.data_k_saldo = rok + "-05-31";
            Saldo_Okno.ShowDialog();
        }

        private void Czerwiec_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-06-01";
            Saldo_Okno.data_k_saldo = rok + "-06-30";
            Saldo_Okno.ShowDialog();
        }

        private void Lipiec_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-07-01";
            Saldo_Okno.data_k_saldo = rok + "-07-31";
            Saldo_Okno.ShowDialog();
        }

        private void Sierpien_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-08-01";
            Saldo_Okno.data_k_saldo = rok + "-08-31";
            Saldo_Okno.ShowDialog();
        }

        private void Wrzesien_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-09-01";
            Saldo_Okno.data_k_saldo = rok + "-09-30";
            Saldo_Okno.ShowDialog();
        }

        private void Pazdziernik_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-10-01";
            Saldo_Okno.data_k_saldo = rok + "-10-31";
            Saldo_Okno.ShowDialog();
        }

        private void Listopad_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-11-01";
            Saldo_Okno.data_k_saldo = rok + "-11-30";
            Saldo_Okno.ShowDialog();
        }

        private void Grudzien_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-12-01";
            Saldo_Okno.data_k_saldo = rok + "-12-31";
            Saldo_Okno.ShowDialog();
        }

        private void Podsumowanie_Click(object sender, EventArgs e)
        {
            id_klient = comboBox1.Text.Split('|');
            Saldo_Okno Saldo_Okno = new Saldo_Okno();
            Saldo_Okno.komenda = komenda_finanse;
            Saldo_Okno.id_klient_saldo = int.Parse(id_klient[0]);
            Saldo_Okno.data_p_saldo = rok + "-01-01";
            Saldo_Okno.data_k_saldo = rok + "-12-31";
            Saldo_Okno.ShowDialog();
        }
    }
}
