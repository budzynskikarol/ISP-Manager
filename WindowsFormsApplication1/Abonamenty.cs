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
    public partial class Abonamenty : Form
    {
        Regex cena = new Regex(@"^[0-9]+\,[0-9]{2}$");
        int row_count = 0;
        bool wszystko_ok;
        int abo_ok, nazwa_ok;
        string[] predkosc_dl, predkosc_ul;

        public Abonamenty()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox2.Select();
        }

        private void Abonamenty_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            resetAll();
        }

        private void Abonamenty_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Subscriptions.* FROM Subscriptions", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Prędkość download";
            dataGridView1.Columns[3].HeaderCell.Value = "Prędkść upload";
            dataGridView1.Columns[4].HeaderCell.Value = "Cena";
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
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
                    msg = "Pomyślnie dodano abonament!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Predkosc_DL", System.Data.SqlDbType.NVarChar).Value = textBox3.Text + " Mbit/s";
                    command.Parameters.Add("@Predkosc_UL", System.Data.SqlDbType.NVarChar).Value = textBox4.Text + " Mbit/s";
                    command.Parameters.Add("@Cena", System.Data.SqlDbType.Decimal).Value = Decimal.Parse(textBox5.Text);
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano abonament!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Subscriptions", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Predkosc_DL", System.Data.SqlDbType.NVarChar).Value = textBox3.Text + " Mbit/s";
                    command.Parameters.Add("@Predkosc_UL", System.Data.SqlDbType.NVarChar).Value = textBox4.Text + " Mbit/s";
                    command.Parameters.Add("@Cena", System.Data.SqlDbType.Decimal).Value = Decimal.Parse(textBox5.Text);
                    break;
                case 2:
                    msg = "Pomyślnie usunięto abonament!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Subscriptions", System.Data.SqlDbType.Int).Value = 
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
                label15.Visible = true;
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

            if (!cena.IsMatch(textBox5.Text))
            {
                wszystko_ok = false;
                label14.Visible = true;
            }

        }

        private void resetErrorLabels()
        {
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (row_count > 0)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    predkosc_dl = dataGridView1.CurrentRow.Cells[2].Value.ToString().Split(null);
                    predkosc_ul = dataGridView1.CurrentRow.Cells[3].Value.ToString().Split(null);
                    textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox3.Text = predkosc_dl[0];
                    textBox4.Text = predkosc_ul[0];
                    textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

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
                SqlCommand command = new SqlCommand("SELECT Subscriptions.* FROM Subscriptions WHERE Id_Subscriptions like '"
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
                SqlCommand command = new SqlCommand("SELECT Subscriptions.* FROM Subscriptions WHERE Nazwa like '"
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
                    SqlCommand command = new SqlCommand("SELECT Subscriptions.* FROM Subscriptions WHERE Cena like '"
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
                SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Subscriptions WHERE Nazwa like '"
                        + textBox2.Text + "'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                
                while (dr.Read())
                {
                    nazwa_ok = (int)dr[0];
                }
                dr.Close();
                
                if (nazwa_ok > 0)
                {
                    wszystko_ok2 = false;
                    MessageBox.Show("Istnieje abonament o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                            
                if (wszystko_ok2)
                {
                    String sql = "INSERT INTO Subscriptions (Nazwa, Predkosc_DL, Predkosc_UL, Cena) " +
                                    "VALUES (@Nazwa, @Predkosc_DL, @Predkosc_UL, @Cena)";
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
                    SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Subscriptions WHERE Nazwa like '"
                        + textBox2.Text + "'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();
                
                    while (dr.Read())
                    {
                        nazwa_ok = (int)dr[0];
                    }
                    dr.Close();
                
                    if (nazwa_ok > 0)
                    {
                        wszystko_ok2 = false;
                        MessageBox.Show("Istnieje abonament o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (wszystko_ok2)
                {
                    String sql = "UPDATE Subscriptions SET Nazwa = @Nazwa, Predkosc_DL = @Predkosc_DL, Predkosc_UL = @Predkosc_UL, " +
                        "Cena = @Cena WHERE Id_Subscriptions = @Id_Subscriptions";
                        this.updateDatabase(sql, 1);
                        resetAll();
                }
            }
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(Id_Contracts) AS Expr1 FROM Contracts WHERE " +
                "Id_Subscriptions = @Id_Subscriptions", setConnection.con);
            command.Parameters.Clear();
            command.Parameters.Add("@Id_Subscriptions", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
            SqlDataReader rdr = command.ExecuteReader();
           
            while (rdr.Read())
            {
                abo_ok = (int)rdr[0];
            }
            rdr.Close();
            
            if (abo_ok > 0)
            {
                MessageBox.Show("Abonament przypisany do umowy. Nie można usunąć!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Usunąć abonament: " + textBox2.Text +
                    ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                    + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    String sql = "DELETE FROM Subscriptions " +
                    "WHERE Id_Subscriptions = @Id_Subscriptions";
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
