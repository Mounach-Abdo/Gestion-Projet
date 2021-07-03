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

namespace GESTION_DE_PROJET.Views.Managa_Project.Forms
{
    public partial class frmAddEditProjet : Form
    {
        public frmAddEditProjet()
        {
            InitializeComponent();
        }

        public string status;
        public string code;
        private void frmAddEditProjet_Load(object sender, EventArgs e)
        {
            try
            {
                if ( App.RoleUSer == App.SecretariatRole )
                {
                    chefProject.Enabled = false;
                    txtMontant.Enabled = false;
                    BtnChooseFiles.Enabled = false;
                    gridFiles.Enabled = false;
                }
                App.FillCombo(ref chefProject, "Select Code, Nom+' '+Prenom as nom from Utilisateur", "Code", "nom");
                App.FillCombo(ref OrganismeClient, "Select Code, Nom from Organisme", "Code", "Nom");
                if ( status == "add" )
                {
                    btnSave.Text = "Ajouter";
                }
                else
                {
                    btnSave.Text = "Modifier";
                    fillFields(code);
                }
            }
            catch ( Exception ex )
            {

            }

        }

        private void fillFields(string code)
        {
            try
            {
                var sql = "Select * from Projet where code='" + code + "'";
                var data = Connection.GetdDataFromDatabase(sql);
                if ( data.Rows.Count > 0 )
                {
                    var row = data.Rows[0];
                    textCode.Text = code;
                    txtNom.Text = row["nom"] + "";
                    txtDesc.Text = row["description"].ToString();
                    chefProject.SelectedValue = row["chefprojet"].ToString();
                    OrganismeClient.SelectedValue = row["clientOrganismeID"].ToString();
                    DateTime.TryParse(row["datedebut"] + "", out var debut);
                    DateTime.TryParse(row["datefin"] + "", out var fin);

                    dateDebut.Value = debut;
                    dateFin.Value = fin;
                    sql = "Select id,chemin from documentTechnique where idprojet='" + code + "'";
                    var documentOfProject = Connection.GetdDataFromDatabase(sql);
                    gridFiles.DataSource = documentOfProject;

                }
            }
            catch ( Exception ex )
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<string> deletedFilesIds = new List<string>();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if ( status == "add" )
            {
                var sql = "INSERT INTO projet";
            }
            else
            {
                //salina m3a modification 
                var sql = "Update Projet Set nom='" + txtNom.Text + "',description='" + txtDesc.Text +
                          "',clientOrganismeID='" + OrganismeClient.SelectedValue + "',datedebut='" +
                          dateDebut.Value.ToString("dd/MM/yyyy HH:mm:ss") + "',datefin='" + dateFin.Value.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                if ( App.RoleUSer == App.DirectionRole )
                {
                    sql += ", chefprojet='" + chefProject.SelectedValue + "',montant='" + txtMontant.Text + "'";
                }

                sql += " where code='" + code + "'";
                var x = Connection.UpdateDatabase(sql);

                if ( x != -1 )
                {
                    if ( App.RoleUSer == App.DirectionRole )
                    {
                        foreach ( DataGridViewRow row in gridFiles.Rows )
                        {
                            var filesSQl = "update documentTechnique set chemin='" + row.Cells[1].Value + "' where id='" + row.Cells[0].Value + "'";
                            Connection.UpdateDatabase(filesSQl);
                        }

                        if ( deletedFilesIds.Count != 0 )
                        {
                            var sqlDelete = "delete from documentTechnique where id in(-1";
                            foreach ( var id in deletedFilesIds )
                                sqlDelete += "," + id;
                            sqlDelete += ")";
                            Connection.UpdateDatabase(sqlDelete);
                        }
                    }

                    MessageBox.Show("les donnes sont bien modifiées", "enregistrement des données",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Close();
                }
                else
                {
                    var result = MessageBox.Show("une erreur s'est produite lors de la modification du projet \r\n Voulez Vous Fermer cette Fenétre", "erreur d'enregistrement des données",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if ( result == DialogResult.Yes )
                        this.Close();

                }

            }


        }
    }
}
