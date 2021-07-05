using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunifu.UI.WinForms;

namespace GESTION_DE_PROJET.Shared
{
   public  class App
    {
        public static string RoleUSer="";
        public static string ConnectedUserId;
        public const string SecretariatRole = "secretariat";
        public const string DirectionRole = "direction";
        public const string formatDateSql = "dd/MM/yyyy HH:mm:ss";

        public static void FillCombo(ref BunifuDropdown combo, string sql, string valueMember, string displayMember)
        {
            try
            {
                var data = Database.GetdDataFromDatabase(sql);
                combo.ValueMember = valueMember;
                combo.DisplayMember = displayMember;
                combo.DataSource = data;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
