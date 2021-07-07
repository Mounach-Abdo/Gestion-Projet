using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GESTION_DE_PROJET.Shared;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GESTION_DE_PROJET.Views.Managa_Project.Forms
{
    public partial class frmAddEditProjet : Form
    {
        public frmAddEditProjet(string status,string code)
        {
            InitializeComponent();
            this.status = status;
            this.code = code;
        }

        public string status;
        public string code;

        public bool verifierChamps()
        {
            if (string.IsNullOrEmpty(txtNom.Text))
            {
                MessageBox.Show("Le nom est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNom.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                MessageBox.Show("La description est requis", "Erreur de saisi", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtDesc.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(dateDebut.Text))
            {
                MessageBox.Show("La date de début est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateDebut.Focus();
                return false;
            }
            if ( string.IsNullOrEmpty(dateFin.Text) )
            {
                MessageBox.Show("La date de fin est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateFin.Focus();
                return false;
            }
            if ( string.IsNullOrEmpty(OrganismeClient.SelectedValue+"") )
            {
                MessageBox.Show("l'organisme client est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OrganismeClient.Focus();
                return false;
            }

            if (App.RoleUSer == App.DirectionRole)
            {
                if ( string.IsNullOrEmpty(txtMontant.Text) )
                {
                    MessageBox.Show("le Montant du projet est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMontant.Focus();
                    return false;
                }
                if ( string.IsNullOrEmpty(chefProject.SelectedValue + "") )
                {
                    MessageBox.Show("le chef du projet est requis", "Erreur de saisi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chefProject.Focus();
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }
        }

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
                var data = Database.GetdDataFromDatabase(sql);
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
                    var documentOfProject = Database.GetdDataFromDatabase(sql);
                    gridFiles.DataSource = documentOfProject;
                    gridFiles.Columns[0].Visible = false;
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
                if (verifierChamps() == false)
                    return;
                var sqlMax = "Select max(Code) from projet";
                var maxCode = Database.GetOneRow(sqlMax);
                var insertedCode = 0;

                if ( string.IsNullOrEmpty(maxCode) || maxCode == "0" )
                    insertedCode = 1;
                else
                    insertedCode++;
                textCode.Text = insertedCode + "";
                var sql = "INSERT INTO projet(Code, nom, description, datedebut, datefin, clientOrganismeID";
                if ( App.RoleUSer == App.DirectionRole )
                {
                    sql += ",montant,chefprojet";
                }

                sql += ") VALUES ('" + textCode.Text + "','" + txtNom.Text + "','" + txtDesc.Text + "','" +
                       dateDebut.Value.ToString(App.formatDateSql) + "','" + dateFin.Value.ToString(App.formatDateSql) +
                       "','" + OrganismeClient.SelectedValue + "'";
                if ( App.RoleUSer == App.DirectionRole )
                {
                    sql += ",'" + txtMontant.Text + "','" + chefProject.SelectedValue + "'";
                }

                sql += ")";

                var x = Database.UpdateDatabase(sql);

                if ( x > 0 )
                {
                    if ( App.RoleUSer == App.DirectionRole )
                    {
                        if ( gridFiles.RowCount > 0 )
                        {
                            sql = "INSERT INTO documentTechnique(chemin,idprojet) VALUES";
                            foreach ( DataGridViewRow row in gridFiles.Rows )
                            {
                                sql += "('" + row.Cells[1].Value + "','" + textCode.Text + "')";
                            }
                            //insert files in database;
                            Database.UpdateDatabase(sql);
                        }
                    }

                    MessageBox.Show("Le Projet est bien ajouté", "Enregistrement des données", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    var result = MessageBox.Show("une erreur s'est produite lors de l'ajout d'un nouvel projet \r\n Voulez Vous Fermer cette Fenétre", "erreur d'enregistrement des données",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if ( result == DialogResult.Yes )
                        this.Close();
                }
            }
            else
            {
                if ( verifierChamps() == false )
                    return;

                //salina m3a modification 
                var sql = "Update Projet Set nom='" + txtNom.Text + "',description='" + txtDesc.Text +
                          "',clientOrganismeID='" + OrganismeClient.SelectedValue + "',datedebut='" +
                          dateDebut.Value.ToString("dd/MM/yyyy HH:mm:ss") + "',datefin='" + dateFin.Value.ToString("dd/MM/yyyy HH:mm:ss") + "'";
                if ( App.RoleUSer == App.DirectionRole )
                {
                    sql += ", chefprojet='" + chefProject.SelectedValue + "',montant='" + txtMontant.Text + "'";
                }

                sql += " where code='" + code + "'";
                var x = Database.UpdateDatabase(sql);

                if ( x != -1 )
                {
                    if ( App.RoleUSer == App.DirectionRole )
                    {
                        foreach ( DataGridViewRow row in gridFiles.Rows )
                        {
                            var filesSQl = "update documentTechnique set chemin='" + row.Cells[1].Value + "' where id='" + row.Cells[0].Value + "'";
                            Database.UpdateDatabase(filesSQl);
                        }

                        if ( deletedFilesIds.Count != 0 )
                        {
                            var sqlDelete = "delete from documentTechnique where id in(-1";
                            foreach ( var id in deletedFilesIds )
                                sqlDelete += "," + id;
                            sqlDelete += ")";
                            Database.UpdateDatabase(sqlDelete);
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

        private void BtnChooseFiles_Click(object sender, EventArgs e)
        {
            var dialog = openFileDialog1.ShowDialog();
            if ( dialog == DialogResult.OK )
            {
                if ( openFileDialog1.FileNames?.Length > 0 )
                {
                    var files = openFileDialog1.FileNames;
                    foreach ( var file in files )
                    {
                        if ( File.Exists(file) )
                            gridFiles.Rows.Add(DBNull.Value, file);
                    }
                }
                else
                {
                    if ( File.Exists(openFileDialog1.FileName) )
                        gridFiles.Rows.Add(DBNull.Value, openFileDialog1.FileName);
                }
            }
        }

        private void gridFiles_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void gridFiles_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var dialog = MessageBox.Show("Voulez Vous Vraiment Supprimer ce fichier?", "Confirmation", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if ( dialog == DialogResult.No )
                e.Cancel = true;
        }

        private void gridFiles_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if ( deletedFilesIds is null )
                deletedFilesIds = new List<string>();
            deletedFilesIds.Add(e.Row.Cells[0].Value + "");
        }
    }
}
