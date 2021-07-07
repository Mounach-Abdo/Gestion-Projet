using GESTION_DE_PROJET.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTION_DE_PROJET.Views.Manage_Users
{
    public partial class AdminUserManagerForm : Form
    {
        public AdminUserManagerForm()
        {
            InitializeComponent();
        }

        //pour ajouter

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser("add", "");
            frm.ShowDialog();
            frm.Close();

        }

        //ppour modifier
        private void bunifuDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex!=-1 && e.RowIndex != -1 )
            {
                var code = bunifuDataGridView1.Rows[e.RowIndex].Cells[1].Value+"";
                frmAddEditUser frm = new frmAddEditUser("edit", code);
                frm.ShowDialog();
                frm.Close();
            }
        }

        private void AdminUserManagerForm_Load(object sender, EventArgs e)
        {
            var sql = "Select * from utilisateur";

            var data = Database.GetdDataFromDatabase(sql);
            bunifuDataGridView1.DataSource = data;
        }
    }
}
