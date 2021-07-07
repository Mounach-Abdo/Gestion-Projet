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
    public partial class frmAddEditUser : Form
    {
        public frmAddEditUser(string action,string code)
        {
            InitializeComponent();
            this.action = action;
            this.code = code;
        }
        public string action;
        public string code; 
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(action== "add" )
            {
                var sql = "insert into utilisateur VALUES('"+textMatrcile.Text+"','"+txtNom.Text+"','"+textPrenom.Text+"','"+textAdress.Text+"','"+textTelephone.Text+"','"+txtNomContact.Text+"','"+textEmail.Text+"','"+RoleEmploye.SelectedValue+"','"+textPassword.Text+"')";
                var x = Database.UpdateDatabase(sql);
                if(x > 0 )
                {
                    MessageBox.Show("l'utilisateur est bien inséré .", "operation succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("un erreur est survenu lors de l'insertion du nouvel utilisateur", "operation echouée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else
            {
                var sql = "update utilisateur set nom='" + txtNom.Text + "',prenom='" + textPrenom.Text + "',address='" + textAdress.Text + "',phone='" + textTelephone.Text + "',nomContact='" + txtNomContact.Text + "',mail='" + textEmail.Text + "',idrole='" + RoleEmploye.SelectedValue + "',password='"+textPassword.Text+"' where matricule='"+textMatrcile.Text+"'";

                var x = Database.UpdateDatabase(sql);
                if ( x > 0 )
                {
                    MessageBox.Show("l'utilisateur est bien modifié .", "operation succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("un erreur est survenu lors de la modifications  d'utilisateur", "operation echouée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            if(action == "add" )
            {

            }
            else
            {
                textMatrcile.Enabled = false;
                GetUser();
            }
        }
        void GetUser()
        {
            var sql = "Select * from utilisateur where matricule='" + code + "'";
            var data = Database.GetdDataFromDatabase(sql);
            if(data.Rows.Count > 0 )
            {
                var row = data.Rows[0];
                textMatrcile.Text = code;
                txtNom.Text = row["nom"]+"";
                textPrenom.Text = row["prenom"]+"";
                textAdress.Text = row["adresse"]+"";
                textTelephone.Text = row["phone"]+"";
                txtNomContact.Text = row["nomcontact"]+"";
                textEmail.Text = row["mail"]+"";
                RoleEmploye.SelectedValue = row["idrole"]+"";
                textPassword.Text = row["password"]+"";
            }
        }
    }
}
