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
            refresh();
        }

        //ppour modifier
        private void bunifuDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex != -1 && e.RowIndex != -1 )
            {
                var code = bunifuDataGridView1.Rows[e.RowIndex].Cells[1].Value+"";
                frmAddEditUser frm = new frmAddEditUser("edit", code);
                frm.ShowDialog();
                frm.Close();
                frm.Dispose();
                refresh();
            }
        }
        void refresh()
        {
            var sql = "Select * from utilisateur";
            var data = Database.GetdDataFromDatabase(sql);
            bunifuDataGridView1.DataSource = data;
            
        }
        private void AdminUserManagerForm_Load(object sender, EventArgs e)
        {
            bunifuDataGridView1.Columns.Insert(0, new DataGridViewCheckBoxColumn() { HeaderText = "Selectionner" });
            refresh();
        }
        List<string> deletedIds = new List<string>();

        private void SelectAll_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if ( SelectAll.Checked )
            {
                deletedIds.Clear();
                foreach ( DataGridViewRow row in bunifuDataGridView1.Rows )
                { 
                    deletedIds.Add(row.Cells[1].Value + "");
                    row.Cells[0].Value = true;
                }
            }
            else
            {
                deletedIds.Clear();
                foreach ( DataGridViewRow row in bunifuDataGridView1.Rows )
                {
                    row.Cells[0].Value = false;
                }
            }
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1 && e.ColumnIndex != -1 && e.ColumnIndex == 0 )
            {
                deletedIds.Add(bunifuDataGridView1.Rows[e.RowIndex].Cells[1].Value + "");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var sql = "Delete from utilisateur where matricule in(-1";
            foreach ( var id in deletedIds )
                sql += ",'" + id+"'";
            sql += ")";

            var x= Database.UpdateDatabase(sql);
            if(x > 0 )
            {
                MessageBox.Show("les utilisateur selectionés sont bien supprimé","suppression avec succes",MessageBoxButtons.OK,MessageBoxIcon.Information);
                refresh();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
