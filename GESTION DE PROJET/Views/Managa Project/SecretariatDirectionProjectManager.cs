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

        }

        private void bunifuPanel1_Click(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            var sql =
                "SELECT Code , nom, description, o.Nom,dateDebut,dateFin from Projet p inner join Organisme o on o.Code==p.clientOrganismeID   ";

            Search search = new Search() {sql = sql};
            search.checkCode.Tag = "p.Code";
            search.lblCode.Text = " Code du projet : ";
            search.CheckNom.Tag = " p.Nom";
            search.lblNom.Text = "Nom du Projet";
            search.CheckOrganisme.Tag = "o.Code";
            search.nomOrganisme.Text = "Organisme Client : ";

            search.title = "Rechercher Un Projet";

            search.orderBy = " order By DateDebut,DateFin";

            search.ShowDialog();

            search.gridResults.CellDoubleClick += (se, ev) =>
            {
                if (ev.RowIndex != 0 && ev.ColumnIndex != 0)
                {
                    var code = search.gridResults.Rows[ev.RowIndex].Cells[0].Value + "";
                    rechercherProject(code);
                    search.Close();
                }
            };

            search.Close();

        }

        private void rechercherProject(string code = "")
        {
            try
            {
                var sql =
                    "SELECT Code , nom, description, o.Nom,dateDebut as 'Date Debut',dateFin as 'Date de fin', Montant, u.Nom as 'Chef De projet',o.Nom as 'organisme Client' from Projet p inner join Organisme o on o.Code==p.clientOrganismeID inner join utilisateur u on p.chefprojet = u.code  ";

                if ( !string.IsNullOrEmpty(code) )
                    sql += " WHERE p.code='" + code + "'";
                var data = Connection.GetdDataFromDatabase(sql);
                gridProject.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("un erreur s'est produit.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }
    }
}
