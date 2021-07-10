using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GESTION_DE_PROJET.Shared;
using GESTION_DE_PROJET.Shared.Recherche;
using GESTION_DE_PROJET.Views.Managa_Project.Forms;

namespace GESTION_DE_PROJET
{
    public partial class SecretariatDirectionProjectManager : Form
    {
        public SecretariatDirectionProjectManager()
        {
            InitializeComponent();
            //sample data
         
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuVScrollBar1_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
           
           
        }

        private void grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           
        }

        private void grid_RowUnshared(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void grid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
           
            
        }

        private void SecretariatDirectionProjectManager_Load(object sender, EventArgs e)
        {
            rechercherProject();
            gridProject.Columns.Insert(0, new DataGridViewCheckBoxColumn() { HeaderText = "Selectionner" });
        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var sql =
                    "SELECT p.Code , p.nom, p.description, o.Nom,dateDebut,dateFin from Projet p inner join Organisme o on o.Code=p.clientOrganismeID   ";

                Search search = new Search() { sql = sql };
                search.checkCode.Tag = "p.Code";
                search.lblCode.Text = " Code du projet : ";
                search.CheckNom.Tag = " p.Nom";
                search.lblNom.Text = "Nom du Projet";
                search.CheckOrganisme.Tag = "o.Nom";
                search.nomOrganisme.Text = "Organisme Client : ";

                search.title = "Rechercher Un Projet";

                search.orderBy = " order By DateDebut,DateFin";

                search.ShowDialog();

                search.gridResults.CellDoubleClick += (se, ev) =>
                {
                    if ( ev.RowIndex != 0 && ev.ColumnIndex != 0 )
                    {
                        var code = search.gridResults.Rows[ev.RowIndex].Cells[0].Value + "";
                        rechercherProject(code);
                        search.Close();
                    }
                };

                search.Close();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// tested
        /// </summary>
        /// <param name="code"></param>
        private void rechercherProject(string code = "")
        {
            try
            {
                var sql =
                    "SELECT p.Code , p.nom, p.description, o.Nom,dateDebut as 'Date Debut',dateFin as 'Date de fin', p.Montant, u.Nom as 'Chef De projet',o.Nom as 'organisme Client' from Projet p inner join Organisme o on o.Code=p.clientOrganismeID inner join utilisateur u on p.chefprojet = u.matricule    ";

                if ( !string.IsNullOrEmpty(code) )
                    sql += " WHERE p.code='" + code + "'";
                var data = Database.GetdDataFromDatabase(sql);
                gridProject.DataSource = data;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("un erreur s'est produit.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void gridProject_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
           
        }

        private void gridProject_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditProjet frm = new frmAddEditProjet("add","") {status = "add"};
            frm.ShowDialog();
            rechercherProject();
        }

        private void gridProject_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex != -1)
            {
                frmAddEditProjet frm = new frmAddEditProjet("edit", gridProject.Rows[e.RowIndex].Cells[1].Value + "");
                frm.ShowDialog();
                rechercherProject();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(ids.Count > 0 )
            {
                var sql = "delete from project where code in (0";
                foreach ( var id in ids )
                    sql += ",'" + id + "'";
                sql += ")";
                Database.UpdateDatabase(sql);
                ids.Clear();
            }
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if ( bunifuCheckBox1.Checked )
            {
                ids.Clear();

                foreach ( DataGridViewRow row in gridProject.Rows )
                {
                    row.Cells[0].Value = true;
                    ids.Add(row.Cells[1].Value + "");
                }
            }
            else
            {
                ids.Clear();

                foreach ( DataGridViewRow row in gridProject.Rows )
                {
                    row.Cells[0].Value = false;
                }
            }
        }

        List<string> ids = new List<string>();

        private void gridProject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1 && e.ColumnIndex ==0 )
            {
                ids.Add(gridProject.Rows[e.RowIndex].Cells[1].Value + "");
            }
        }
    }
}
