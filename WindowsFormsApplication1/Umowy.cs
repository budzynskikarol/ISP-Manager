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
    public partial class Umowy : Form
    {
        int row_count = 0;
        bool wszystko_ok;
        string[] id_klient, id_abonament = null;
        int numer_ok;

        public Umowy()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
        }

        private void Umowy_Load(object sender, EventArgs e)
        {
            fillcombo();
            updateDataGrid();
            resetAll();
        }

        private void Umowy_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                "Subscriptions.Id_Subscriptions", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id umowy";
            dataGridView1.Columns[1].HeaderCell.Value = "Numer";
            dataGridView1.Columns[2].HeaderCell.Value = "Data zawarcia";
            dataGridView1.Columns[3].HeaderCell.Value = "Czas trwania";
            dataGridView1.Columns[4].HeaderCell.Value = "Id klienta";
            dataGridView1.Columns[5].HeaderCell.Value = "Imię";
            dataGridView1.Columns[6].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[7].HeaderCell.Value = "Id abonamentu";
            dataGridView1.Columns[8].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[9].HeaderCell.Value = "Cena";
            dataGridView1.Columns[10].HeaderCell.Value = "Obowiązująca";
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox4.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            dateTimePicker1.ResetText();
            checkBox1.Checked = false;

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
                    msg = "Pomyślnie dodano umowę!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Numer", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Data_zawarcia", System.Data.SqlDbType.Date).Value = dateTimePicker1.Text;
                    command.Parameters.Add("@Czas_trwania", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Subscriptions", System.Data.SqlDbType.Int).Value = int.Parse(id_abonament[0]);
                    command.Parameters.Add("@Obowiazujaca", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox1.Checked.ToString());
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano umowę!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Contracts", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Numer", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Data_zawarcia", System.Data.SqlDbType.Date).Value = dateTimePicker1.Text;
                    command.Parameters.Add("@Czas_trwania", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Subscriptions", System.Data.SqlDbType.Int).Value = int.Parse(id_abonament[0]);
                    command.Parameters.Add("@Obowiazujaca", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox1.Checked.ToString());
                    break;
                case 2:
                    msg = "Pomyślnie usunięto umowę!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Contracts", System.Data.SqlDbType.Int).Value =
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

            if (textBox2.Text == "")
            {
                wszystko_ok = false;
                label4.Visible = true;
            }

            if (textBox4.Text == "")
            {
                wszystko_ok = false;
                label13.Visible = true;
            }

            if (comboBox3.Text == "")
            {
                wszystko_ok = false;
                label17.Visible = true;
            }

            if (comboBox2.Text == "")
            {
                wszystko_ok = false;
                label19.Visible = true;
            }
        }

        private void resetErrorLabels()
        {
            label4.Visible = false;
            label13.Visible = false;
            label17.Visible = false;
            label19.Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (row_count > 0)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    comboBox3.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[5].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[6].Value.ToString();
                    comboBox2.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[8].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[9].Value.ToString();
                    checkBox1.Checked =  bool.Parse(dataGridView1.CurrentRow.Cells[10].Value.ToString());

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                }
            }
        }

        private void fillcombo()
        {
            SqlCommand command = new SqlCommand("SELECT Subscriptions.* FROM Subscriptions", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string sName = dr.GetInt32(0) + " | " + dr.GetString(1) + " | " + dr.GetDecimal(4);
                comboBox2.Items.Add(sName);
            }
            dr.Close();

            command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Id_Clients NOT LIKE 1", setConnection.con);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                string sName = dr.GetInt32(0) + " | " + dr.GetString(2) + " | " + dr.GetString(3);
                comboBox3.Items.Add(sName);
            }
            dr.Close();
        }

        private void wyszukiwanie_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Id umowy")
            {
            SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                "Subscriptions.Id_Subscriptions WHERE Contracts.Id_Contracts like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Numer")
            {
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Contracts.Numer like '"
                        + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Id klienta")
            {
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Clients.Id_Clients like '"
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
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Clients.Imie like '"
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
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Clients.Nazwisko like '"
                        + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Id abonamentu")
            {
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Subscriptions.Id_Subscriptions like '"
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
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Subscriptions.Nazwa like '"
                        + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Cena")
            {
                if (textBox1.Text != "")
                {
                    SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                        "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                        "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                        "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                        "Subscriptions.Id_Subscriptions WHERE Subscriptions.Cena like '"
                            + textBox1.Text + "%'", setConnection.con);
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
            else if (comboBox1.Text == "Obowiązująca")
            {
                SqlCommand command = new SqlCommand("SELECT Contracts.Id_Contracts, Contracts.Numer, Contracts.Data_zawarcia, " +
                    "Contracts.Czas_trwania, Contracts.Id_Clients, Clients.Imie, Clients.Nazwisko, Contracts.Id_Subscriptions, " +
                    "Subscriptions.Nazwa, Subscriptions.Cena, Contracts.Obowiazujaca FROM Clients INNER JOIN Contracts ON " +
                    "Clients.Id_Clients = Contracts.Id_Clients INNER JOIN Subscriptions ON Contracts.Id_Subscriptions = " +
                    "Subscriptions.Id_Subscriptions WHERE Contracts.Obowiazujaca like '"
                        + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }

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
                SqlCommand command = new SqlCommand("SELECT COUNT(Numer) AS Expr1 FROM Contracts WHERE Numer like '"
                        + textBox2.Text + "'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    numer_ok = (int)dr[0];
                }
                dr.Close();

                if (numer_ok > 0)
                {
                    wszystko_ok2 = false;
                    MessageBox.Show("Istnieje umowa o podanym numerze!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                

                if (wszystko_ok2)
                {
                    String sql = "INSERT INTO Contracts (Numer, Data_zawarcia, Czas_trwania, Id_Clients, Id_Subscriptions, Obowiazujaca) " +
                                    "VALUES (@Numer, @Data_zawarcia, @Czas_trwania, @Id_Clients, @Id_Subscriptions, @Obowiazujaca)";
                    id_klient = comboBox3.Text.Split('|');
                    id_abonament = comboBox2.Text.Split('|');
                    this.updateDatabase(sql, 0);
                    resetAll();
                }
            }
        }

        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                bool wszystko_ok2 = true;
                
                if (!(string.Equals(textBox2.Text, dataGridView1.CurrentRow.Cells[1].Value.ToString())))
                {
                    SqlCommand command = new SqlCommand("SELECT COUNT(Numer) AS Expr1 FROM Contracts WHERE Numer like '"
                        + textBox2.Text + "'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        numer_ok = (int)dr[0];
                    }
                    dr.Close();
                    
                    if (numer_ok > 0)
                    {
                        wszystko_ok2 = false;
                        MessageBox.Show("Istnieje umowa o podanym numerze!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                
                }

                if (wszystko_ok2)
                {
                    String sql = "UPDATE Contracts SET Numer = @Numer, Data_zawarcia = @Data_zawarcia, Czas_trwania = @Czas_trwania, " +
                        "Id_Clients = @Id_Clients, Id_Subscriptions = @Id_Subscriptions, Obowiazujaca = @Obowiazujaca " +
                        "WHERE Id_Contracts = @Id_Contracts";
                    id_klient = comboBox3.Text.Split('|');
                    id_abonament = comboBox2.Text.Split('|');
                    this.updateDatabase(sql, 1);
                    resetAll();
                }
            }
        }
        
        private void Usun_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Usunąć umowę numer: " + textBox2.Text +
                ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                String sql = "DELETE FROM Contracts " +
                "WHERE Id_Contracts = @Id_Contracts";
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
    }
}
