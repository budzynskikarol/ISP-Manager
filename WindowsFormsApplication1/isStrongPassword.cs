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
    class isStrongPassword
    {
        public static bool Password(string password)
        {
            // dlugosc hasla min 8
            if (password.Length < 8)
                return false;

            // Znaki specjalne min 1
            if (!(password.Contains("!") || password.Contains("@") || password.Contains("#") || password.Contains("$") ||
                password.Contains("%") || password.Contains("^") || password.Contains("&") || password.Contains("*") ||
                password.Contains("(") || password.Contains(")") || password.Contains("-") || password.Contains("_") ||
                password.Contains("+") || password.Contains("=")))
                return false;

            // Inne znaki niz !@#$%^&*()_+-=
            if (password.Contains("`") || password.Contains("{") || password.Contains("[") || password.Contains("}") ||
                password.Contains("]") || password.Contains(@"\") || password.Contains("|") || password.Contains('"') ||
                password.Contains("'") || password.Contains(":") || password.Contains(";") || password.Contains("/") ||
                password.Contains("?") || password.Contains(">") || password.Contains(".") || password.Contains(",") ||
                password.Contains("<") || password.Contains(" ")
                )
                return false;

            // wielkie litery min 1
            if (!password.Any(c => char.IsUpper(c)))
                return false;

            return true;
        }
    }
}
