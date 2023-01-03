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
    class setConnection
    {
        public static SqlConnection con = null;
        public static string conn_str = ISP_Manager.Properties.Settings.Default.ISPdbConnectionString;

        public static void Connection()
        {
            con = new SqlConnection(conn_str);
            con.Open();
        }
    }
}
