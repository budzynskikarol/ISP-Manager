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
using System.Net.NetworkInformation;

namespace ISP_Manager
{
    public partial class Urzadzenia : Form
    {
        int row_count, row_count2, urz_ok, nazwa_ok, ip_ok, seryjny_ok = 0;
        bool wszystko_ok;
        string[] id_klient, id_urz, numer_ip = null;

        public Urzadzenia()
        {
            setConnection.Connection();
            InitializeComponent();
            textBox14.Select();
            fillcombo();
        }

        private void Urzadzenia_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            resetAll();
        }

        private void Urzadzenia_FormClosed(object sender, FormClosedEventArgs e)
        {
            setConnection.con.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt.DefaultView;
            row_count2 = dataGridView2.RowCount;
            dr.Close();
            dataGridView2.Columns[0].HeaderCell.Value = "Id";
            dataGridView2.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView2.Columns[2].HeaderCell.Value = "Typ";
            dataGridView2.Columns[3].HeaderCell.Value = "Adres IP";

            command = new SqlCommand("SELECT Devices_C.* FROM Devices_C", setConnection.con);
            dr = command.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            row_count = dataGridView1.RowCount;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Typ";
            dataGridView1.Columns[3].HeaderCell.Value = "Numer seryjny";
            dataGridView1.Columns[4].HeaderCell.Value = "Adres IP";
            dataGridView1.Columns[5].HeaderCell.Value = "Id klienta";
            dataGridView1.Columns[6].HeaderCell.Value = "Id urządzenia operatora";
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
            textBox12.Text = null;
            textBox13.Text = null;
            textBox14.Text = null;
            textBox15.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
            comboBox4.Text = null;
            checkBox2.Checked = false;
            checkBox3.Checked = false;

            Dodaj.Enabled = true;
            Aktualizuj.Enabled = false;
            Usun.Enabled = false;

            updateDataGrid();

            if (row_count > 0)
            {
                dataGridView1.CurrentRow.Selected = false;
            }

            if (row_count2 > 0)
            {
                dataGridView2.CurrentRow.Selected = false;
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
                    msg = "Pomyślnie dodano urządzenie klienta!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox14.Text;
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = textBox13.Text;
                    command.Parameters.Add("@Numer_seryjny", System.Data.SqlDbType.NVarChar).Value = textBox15.Text;
                    command.Parameters.Add("@Adres_IP", System.Data.SqlDbType.NVarChar).Value = textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value = int.Parse(id_urz[0]);
                    break;
                case 1:
                    msg = "Pomyślnie dodano urządzenie operatora!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Adres_IP", System.Data.SqlDbType.NVarChar).Value = textBox6.Text + "." + textBox8.Text + "." + textBox9.Text + "." + textBox10.Text;
                    break;
                case 2:
                    msg = "Pomyślnie zaktualizowano urządzenie klienta!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Devices_C", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox14.Text;
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = textBox13.Text;
                    command.Parameters.Add("@Numer_seryjny", System.Data.SqlDbType.NVarChar).Value = textBox15.Text;
                    command.Parameters.Add("@Adres_IP", System.Data.SqlDbType.NVarChar).Value = textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text;
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = int.Parse(id_klient[0]);
                    command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value = int.Parse(id_urz[0]);

                    break;
                case 3:
                    msg = "Pomyślnie zaktualizowano urządzenie operatora!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value = dataGridView2.CurrentRow.Cells[0].Value;
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@Typ", System.Data.SqlDbType.NVarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Adres_IP", System.Data.SqlDbType.NVarChar).Value = textBox6.Text + "." + textBox8.Text + "." + textBox9.Text + "." + textBox10.Text;
                    break;
                case 4:
                    msg = "Pomyślnie usunięto urządzenie klienta!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Devices_C", System.Data.SqlDbType.Int).Value =
                        dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    break;
                case 5:
                    msg = "Pomyślnie usunięto urządzenie operatora!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value =
                        dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    break;
            }

            int n = command.ExecuteNonQuery();
            if (n > 0)
            {
                MessageBox.Show(msg, "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateDataGrid();
            }
        }

        private bool check_ip(string a, string b, string c, string d)
        {
            label15.Visible = false;
            label24.Visible = false;
            if (!(a == "" || b == "" || c == "" || d == ""))
            {
                if (Int32.Parse(a) > 0 && Int32.Parse(a) < 256 && Int32.Parse(b) < 256 && Int32.Parse(c) < 256
                && Int32.Parse(d) < 256)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void check_boxy()
        {
            if (checkBox2.Checked)
            {
                wszystko_ok = true;
                resetErrorLabels();

                if (textBox14.Text == "")
                {
                    wszystko_ok = false;
                    label20.Visible = true;
                }

                if (textBox13.Text == "")
                {
                    wszystko_ok = false;
                    label18.Visible = true;
                }

                if (!(check_ip(textBox2.Text, textBox11.Text, textBox3.Text, textBox12.Text)))
                {
                    wszystko_ok = false;
                    label15.Visible = true;
                }

                if (textBox15.Text == "")
                {
                    wszystko_ok = false;
                    label23.Visible = true;
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
            else
            if (checkBox3.Checked)
            {
                wszystko_ok = true;
                resetErrorLabels();

                if (textBox7.Text == "")
                {
                    wszystko_ok = false;
                    label5.Visible = true;
                }

                if (textBox5.Text == "")
                {
                    wszystko_ok = false;
                    label12.Visible = true;
                }

                if (!(check_ip(textBox6.Text, textBox8.Text, textBox9.Text, textBox10.Text)))
                {
                    wszystko_ok = false;
                    label24.Visible = true;
                }
            }
        }

        private void resetErrorLabels()
        {
            label5.Visible = false;
            label12.Visible = false;
            label15.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label23.Visible = false;
            label24.Visible = false;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (row_count > 0)
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    numer_ip = (dataGridView1.CurrentRow.Cells[4].Value.ToString()).Split('.');
                    textBox14.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    textBox13.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    textBox15.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    textBox2.Text = numer_ip[0];
                    textBox11.Text = numer_ip[1];
                    textBox3.Text = numer_ip[2];
                    textBox12.Text = numer_ip[3];

                    SqlCommand command = new SqlCommand("SELECT Clients.* FROM Clients WHERE Id_Clients = @Id_Clients", setConnection.con);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[5].Value;
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        string sName = dr.GetInt32(0) + " | " + dr.GetString(2) + " | " + dr.GetString(3);
                        comboBox3.Text = sName;
                    }
                    dr.Close();

                    command = new SqlCommand("SELECT Devices_O.* FROM Devices_O WHERE Id_Devices_O = @Id_Devices_O", setConnection.con);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value = dataGridView1.CurrentRow.Cells[6].Value;
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        string sName = dr.GetInt32(0) + " | " + dr.GetString(1) + " | " + dr.GetString(3);
                        comboBox2.Text = sName;
                    }
                    dr.Close();

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                    checkBox2.Checked = true;
                                       
                    textBox4.Text = null;
                    textBox5.Text = null;
                    textBox6.Text = null;
                    textBox7.Text = null;
                    textBox8.Text = null;
                    textBox9.Text = null;
                    textBox10.Text = null;
                    comboBox4.Text = null;

                    if (row_count2 > 0)
                    {
                        dataGridView2.CurrentRow.Selected = false;
                    }
                }
            }
        }

        private void dataGridView2_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (row_count2 > 0)
            {
                if (dataGridView2.CurrentRow.Index != -1)
                {
                    numer_ip = (dataGridView2.CurrentRow.Cells[3].Value.ToString()).Split('.');
                    textBox7.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                    textBox5.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                    textBox6.Text = numer_ip[0];
                    textBox8.Text = numer_ip[1];
                    textBox9.Text = numer_ip[2];
                    textBox10.Text = numer_ip[3];

                    Dodaj.Enabled = false;
                    Aktualizuj.Enabled = true;
                    Usun.Enabled = true;
                    Wyczysc.Enabled = true;
                    checkBox3.Checked = true;

                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox11.Text = null;
                    textBox12.Text = null;
                    textBox13.Text = null;
                    textBox14.Text = null;
                    textBox15.Text = null;
                    comboBox1.Text = null;
                    comboBox2.Text = null;
                    comboBox3.Text = null;

                    if (row_count > 0)
                    {
                        dataGridView1.CurrentRow.Selected = false;
                    }
                }
            }
        }

        private void fillcombo()
        {
            SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O", setConnection.con);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string sName = dr.GetInt32(0) + " | " + dr.GetString(1) + " | " + dr.GetString(3);
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
            if (comboBox1.Text == "Id")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Id_Devices_C like '"
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
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Nazwa like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Typ")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Typ like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Numer seryjny")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Numer_seryjny like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Adres IP")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Adres_IP like '"
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
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Id_Clients like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
            else if (comboBox1.Text == "Id urzadzenia operatora")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_C.* FROM Devices_C WHERE Id_Devices_O like '"
                    + textBox1.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt.DefaultView;
                dr.Close();
                row_count = dataGridView1.RowCount;
            }
        }

        private void wyszukiwanie2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox4.Text == "Id")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O WHERE Id_Devices_O like '"
                    + textBox4.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt.DefaultView;
                dr.Close();
                row_count2 = dataGridView2.RowCount;
            }
            else if (comboBox4.Text == "Nazwa")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O WHERE Nazwa like '"
                    + textBox4.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt.DefaultView;
                dr.Close();
                row_count2 = dataGridView2.RowCount;
            }
            else if (comboBox4.Text == "Typ")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O WHERE Typ like '"
                    + textBox4.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt.DefaultView;
                dr.Close();
                row_count2 = dataGridView2.RowCount;
            }
            else if (comboBox4.Text == "Adres IP")
            {
                SqlCommand command = new SqlCommand("SELECT Devices_O.* FROM Devices_O WHERE Adres_IP like '"
                    + textBox4.Text + "%'", setConnection.con);
                SqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt.DefaultView;
                dr.Close();
                row_count2 = dataGridView2.RowCount;
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = null;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = null;
        }

        private void OnlyDigits_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkBox2_CheckedStateChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
            }
        }

        private bool PingHost(string hostname)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            options.DontFragment = true;

            string data = "simple data";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 10;
            PingReply reply = pingSender.Send(hostname, timeout, buffer, options);
            
            if (reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (!(checkBox2.Checked) && !(checkBox3.Checked))
            {
                MessageBox.Show("Proszę wybrać urządzenia klientów lub urządzenia operatora.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (checkBox2.Checked)
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;
                    SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Devices_C WHERE Nazwa like '"
                        + textBox14.Text + "'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        nazwa_ok = (int)dr[0];
                    }
                    dr.Close();

                    if (nazwa_ok > 0)
                    {
                        wszystko_ok2 = false;
                        MessageBox.Show("Istnieje urządzednie klienta o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        command = new SqlCommand("SELECT COUNT(Adres_IP) AS Expr1 FROM Devices_C WHERE Adres_IP like '"
                            + textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text + "'", setConnection.con);
                        dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            ip_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (ip_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzenie klienta o podanym numerze IP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            command = new SqlCommand("SELECT COUNT(Numer_seryjny) AS Expr1 FROM Devices_C WHERE Numer_seryjny like '"
                                + textBox15.Text + "'", setConnection.con);
                            dr = command.ExecuteReader();

                            while (dr.Read())
                            {
                                seryjny_ok = (int)dr[0];
                            }
                            dr.Close();

                            if (seryjny_ok > 0)
                            {
                                wszystko_ok2 = false;
                                MessageBox.Show("Istnieje urządzenie klienta o podanym numerze seryjnym!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "INSERT INTO Devices_C (Nazwa, Typ, Numer_seryjny, Adres_IP, Id_Clients, Id_Devices_O) " +
                            "VALUES (@Nazwa, @Typ, @Numer_seryjny, @Adres_IP, @Id_Clients, @Id_Devices_O)";
                        id_klient = comboBox3.Text.Split('|');
                        id_urz = comboBox2.Text.Split('|');
                        this.updateDatabase(sql, 0);
                        resetAll();
                    }
                }
            }
            else if (checkBox3.Checked)
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;
                    SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Devices_O WHERE Nazwa like '"
                        + textBox7.Text + "'", setConnection.con);
                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        nazwa_ok = (int)dr[0];
                    }
                    dr.Close();

                    if (nazwa_ok > 0)
                    {
                        wszystko_ok2 = false;
                        MessageBox.Show("Istnieje urządzednie operatora o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        command = new SqlCommand("SELECT COUNT(Adres_IP) AS Expr1 FROM Devices_O WHERE Adres_IP like '"
                            + textBox6.Text + "." + textBox8.Text + "." + textBox9.Text + "." + textBox10.Text + "'", setConnection.con);
                        dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            ip_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (ip_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzenie operatora o podanym numerze IP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "INSERT INTO Devices_O (Nazwa, Typ, Adres_IP) " +
                            "VALUES (@Nazwa, @Typ, @Adres_IP)";
                        this.updateDatabase(sql, 1);
                        resetAll();
                    }
                }
            }
        }

        private void Aktualizuj_Click(object sender, EventArgs e)
        {
            if (!(checkBox2.Checked) && !(checkBox3.Checked))
            {
                MessageBox.Show("Proszę wybrać urządzenia klientów lub urządzenia operatora.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (checkBox2.Checked)
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;

                    if (!(textBox14.Text == dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Devices_C WHERE Nazwa like '"
                            + textBox14.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            nazwa_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (nazwa_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzednie klienta o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (!((textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text) == dataGridView1.CurrentRow.Cells[4].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Adres_IP) AS Expr1 FROM Devices_C WHERE Adres_IP like '"
                            + textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            ip_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (ip_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzenie klienta o podanym numerze IP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (!(textBox15.Text == dataGridView1.CurrentRow.Cells[3].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Numer_seryjny) AS Expr1 FROM Devices_C WHERE Numer_seryjny like '"
                            + textBox15.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            seryjny_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (seryjny_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzenie klienta o podanym numerze seryjnym!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "UPDATE Devices_C SET Nazwa = @Nazwa, Typ = @Typ, Numer_seryjny = @Numer_seryjny, " +
                            "Adres_IP = @Adres_IP, Id_Clients = @Id_Clients, Id_Devices_O = @Id_Devices_O " +
                            "WHERE Id_Devices_C = @Id_Devices_C";
                        id_klient = comboBox3.Text.Split('|');
                        id_urz = comboBox2.Text.Split('|');
                        this.updateDatabase(sql, 2);
                        resetAll();
                    }
                }
            }
            else if (checkBox3.Checked)
            {
                check_boxy();
                if (wszystko_ok)
                {
                    bool wszystko_ok2 = true;

                    if (!(textBox7.Text == dataGridView2.CurrentRow.Cells[1].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Nazwa) AS Expr1 FROM Devices_O WHERE Nazwa like '"
                            + textBox7.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            nazwa_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (nazwa_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzednie operatora o podanej nazwie!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (!((textBox6.Text + "." + textBox8.Text + "." + textBox9.Text + "." + textBox10.Text) == dataGridView2.CurrentRow.Cells[3].Value.ToString()))
                    {
                        SqlCommand command = new SqlCommand("SELECT COUNT(Adres_IP) AS Expr1 FROM Devices_O WHERE Adres_IP like '"
                            + textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text + "'", setConnection.con);
                        SqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            ip_ok = (int)dr[0];
                        }
                        dr.Close();

                        if (ip_ok > 0)
                        {
                            wszystko_ok2 = false;
                            MessageBox.Show("Istnieje urządzenie operatora o podanym numerze IP!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (wszystko_ok2)
                    {
                        String sql = "UPDATE Devices_O SET Nazwa = @Nazwa, Typ = @Typ, Adres_IP = @Adres_IP " +
                            "WHERE Id_Devices_O = @Id_Devices_O";
                        this.updateDatabase(sql, 3);
                        resetAll();
                    }
                }
            }
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            if (!(checkBox2.Checked) && !(checkBox3.Checked))
            {
                MessageBox.Show("Proszę wybrać urządzenia klientów lub urządzenia operatora.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (checkBox2.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("Usunąć urządzenie klienta: " + textBox14.Text +
                    ", ID " + dataGridView1.CurrentRow.Cells[0].Value.ToString()
                    + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    String sql = "DELETE FROM Devices_C " +
                    "WHERE Id_Devices_C = @Id_Devices_C";
                    updateDatabase(sql, 4);
                    resetAll();
                }
            }
            else if (checkBox3.Checked)
            {
                SqlCommand command = new SqlCommand("SELECT COUNT(Id_Devices_O) AS Expr1 FROM Devices_C WHERE " +
                    "Id_Devices_O = @Id_Devices_O", setConnection.con);
                command.Parameters.Clear();
                command.Parameters.Add("@Id_Devices_O", System.Data.SqlDbType.Int).Value = dataGridView2.CurrentRow.Cells[0].Value;
                SqlDataReader rdr = command.ExecuteReader();
           
                while (rdr.Read())
                {
                    urz_ok = (int)rdr[0];
                }
                rdr.Close();

                if (urz_ok > 0)
                {
                    MessageBox.Show("Do urządzenia operatora przypisane urządzenia klientów. Nie można usunąć!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Usunąć urządzenie operatora: " + textBox7.Text +
                        ", ID " + dataGridView2.CurrentRow.Cells[0].Value.ToString()
                        + "?", "Komunikat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        String sql = "DELETE FROM Devices_O " +
                        "WHERE Id_Devices_O = @Id_Devices_O";
                        updateDatabase(sql, 5);
                        resetAll();
                    }
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
            if (check_ip(textBox6.Text, textBox8.Text, textBox9.Text, textBox10.Text))
            {
                if (PingHost(textBox6.Text + "." + textBox8.Text + "." + textBox9.Text + "." + textBox10.Text))
                {
                    MessageBox.Show("Urządzenie dostępne.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Urządzenie nieosiągalne.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                label24.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (check_ip(textBox2.Text, textBox11.Text, textBox3.Text, textBox12.Text))
            {
                if (PingHost(textBox2.Text + "." + textBox11.Text + "." + textBox3.Text + "." + textBox12.Text))
                {
                    MessageBox.Show("Urządzenie dostępne.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Urządzenie nieosiągalne.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                label15.Visible = true;
            }
        }
    }
}