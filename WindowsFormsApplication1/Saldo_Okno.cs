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
    public partial class Saldo_Okno : Form
    {
        public static string komenda, data_p_saldo, data_k_saldo;
        public static int id_klient_saldo;


        public Saldo_Okno()
        {
            InitializeComponent();
        }

        private void Saldo_Okno_Load(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand(komenda, setConnection.con);
            command.Parameters.Clear();
            command.Parameters.Add("@Id_Clients", System.Data.SqlDbType.Int).Value = id_klient_saldo;
            command.Parameters.Add("@Data_p", System.Data.SqlDbType.Date).Value = data_p_saldo;
            command.Parameters.Add("@Data_k", System.Data.SqlDbType.Date).Value = data_k_saldo;

            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Id";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwa";
            dataGridView1.Columns[2].HeaderCell.Value = "Kwota";
            dataGridView1.Columns[3].HeaderCell.Value = "Typ";
            dataGridView1.Columns[4].HeaderCell.Value = "Id klienta";
            dataGridView1.Columns[4].HeaderCell.Value = "Imię";
            dataGridView1.Columns[4].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[4].HeaderCell.Value = "Data";
        }
    }
}
