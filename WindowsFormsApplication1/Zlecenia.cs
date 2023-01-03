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
    public partial class Zlecenia : Form
    {
        int row_count = 0;
        bool wszystko_ok;
        string[] id_klient, id_pracownik = null;

        public Zlecenia()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
            fillcombo();
        }

        private void Zlecenia_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            resetAll();
        }

        private void Zlecenia_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id zlecenia";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Opis";
            dataGridView1.Columns[3].HeaderCell.Value = "Komentarz";
            dataGridView1.Columns[4].HeaderCell.Value = "Data dodania";
            dataGridView1.Columns[5].HeaderCell.Value = "Data aktualizacji";
            dataGridView1.Columns[6].HeaderCell.Value = "Id klienta";
            dataGridView1.Columns[7].HeaderCell.Value = "Imię";
            dataGridView1.Columns[8].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[9].HeaderCell.Value = "Id pracownika";
            dataGridView1.Columns[10].HeaderCell.Value = "Imię";
            dataGridView1.Columns[11].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[12].HeaderCell.Value = "Status";
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox11.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
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

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, setConnection.con);

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano zgłoszenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = textBox11.Text;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Data_dodania", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Users", System.Data.SqlDbType.Int).Value = int.Parse(id_pracownik[0]);
                    command.Parameters.Add("@Status", System.Data.SqlDbType.Bit).Value = 0;
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano zlecenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Orders", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = textBox11.Text;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Data_aktualizacji", System.Data.SqlDbType.DateTime).Value = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Users", System.Data.SqlDbType.Int).Value = int.Parse(id_pracownik[0]);
                    command.Parameters.Add("@Status", System.Data.SqlDbType.Bit).Value = bool.Parse(checkBox8.Checked.ToString());
                    break;
                case 2:
                    msg = "Pomyślnie usunięto zlecenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Orders", System.Data.SqlDbType.Int).Value =
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
                    textBox11.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    comboBox3.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[7].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[8].Value.ToString();
                    comboBox2.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[10].Value.ToString() + " | " +
                        dataGridView1.CurrentRow.Cells[11].Value.ToString();
                    checkBox8.Checked = bool.Parse(dataGridView1.CurrentRow.Cells[12].Value.ToString());

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                    checkBox8.Visible = true;
                }
            }
        }

        private void fillcombo()
        {
            SqlCommand command = new SqlCommand("SELECT Users.* FROM Users WHERE Id_Users NOT LIKE 1", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string sName = dr.GetInt32(0) + " | " + dr.GetString(3) + " | " + dr.GetString(4);
                comboBox2.Items.Add(sName);
            }
            dr.Close();

            command = new SqlCommand("SELECT Clients.* FROM Clients", setConnection.con);
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
            if (comboBox1.Text == "Id zlecenia")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Orders.Id_Orders like '"
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
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Orders.Nazwa like '"
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
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Clients.Id_Clients like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Imię klienta")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Clients.Imie like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Nazwisko klienta")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Clients.Nazwisko like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Id pracownika")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Users.Id_Users like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Imię pracownika")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Users.Imie like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Nazwisko pracownika")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Users.Nazwisko like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Status")
            {
                SqlCommand command = new SqlCommand("SELECT Orders.Id_Orders, Orders.Nazwa, Orders.Opis, Orders.Komentarz, " +
                "Orders.Data_dodania, Orders.Data_aktualizacji, Clients.Id_Clients, Clients.Imie, " +
                "Clients.Nazwisko, Users.Id_Users, Users.Imie AS Expr1, Users.Nazwisko AS Expr2, Orders.Status FROM Clients " +
                "INNER JOIN Orders ON Clients.Id_Clients = Orders.Id_Clients INNER JOIN Users ON Orders.Id_Users = Users.Id_Users WHERE Orders.Status like '"
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
                String sql = "INSERT INTO Orders (Nazwa, Opis, Komentarz, Data_dodania, Id_Users, Id_Clients, Status) " +
                                    "VALUES (@Nazwa, @Opis, @Komentarz, @Data_dodania, @Id_Users, @Id_Clients, @Status)";
                id_klient = comboBox3.Text.Split('|');
                id_pracownik = comboBox2.Text.Split('|');
                this.updateDatabase(sql, 0);
                resetAll();
            }
        }

        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                String sql = "UPDATE Orders SET Nazwa = @Nazwa, Opis = @Opis, Komentarz = @Komentarz, Data_aktualizacji = @Data_aktualizacji, " +
                    "Id_Clients = @Id_Clients, Id_Users = @Id_Users, Status = @Status " +
                    "WHERE Id_Orders = @Id_Orders";
                id_klient = comboBox3.Text.Split('|');
                id_pracownik = comboBox2.Text.Split('|');
                this.updateDatabase(sql, 1);
                resetAll();
            }
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Usunąć zlecenie: " + textBox2.Text + 
                ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                String sql = "DELETE FROM Orders " +
                "WHERE Id_Orders = @Id_Orders";
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
