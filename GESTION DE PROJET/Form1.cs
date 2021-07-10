using GESTION_DE_PROJET.Shared;
using GESTION_DE_PROJET.Views.Manage_Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTION_DE_PROJET
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bunifuLabel5_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel10_Click(object sender, EventArgs e)
        {

        }

        private void signup_Click(object sender, EventArgs e)
        {
         
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {

        }

        private void slideA_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text))
            {
                MessageBox.Show("Veux devez entrer un login", "erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                login.Focus();
                return;
            }
            bool isMailCorrect = true;
            try
            {
                var mail = new MailAddress(login.Text);
            }

            catch (FormatException)
            {
                isMailCorrect = false;
            }
            if (isMailCorrect == false)
            {
                MessageBox.Show("Le Format d'email Entrer est invalide ", "erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                login.Focus();
                return;
            }
            //var roleInputed = login.Text.Substring(login.Text.IndexOf("@"), login.Text.LastIndexOf(".") - login.Text.IndexOf("@"));

            //var sql = "Select nomrole roleEmployes where nomrole like '" + roleInputed + "'";
            //var result = Database.GetOneRow(sql);
            //if(result == "")
            //{
            //    MessageBox.Show("Le nom de domaine est invalide(Role) ", "erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error
            //       );
            //    login.Focus();
            //    return;
            //}
            if (string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Veux devez un mot de passe", "erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error
                      );

                password.Focus();
                return;

            }

             var sql = "Select * from utilisateur where Mail='" + login.Text + "' and password='" + password.Text + "'";
            var user = Database.GetdDataFromDatabase(sql);

            if (user != null && user.Rows.Count > 0)
            {
                MessageBox.Show("Vous etes connécté \r\n Bienvenue", "Bienvenue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var role = login.Text.Substring(login.Text.IndexOf("@")+1, login.Text.LastIndexOf(".") - login.Text.IndexOf("@")-1);
                App.ConnectedUserId = user.Rows[0]["matricule"] + "";
                App.RoleUSer = role;
                if (role.ToLower() == "Secretariat".ToLower() || role.ToLower() == "Direction".ToLower())
                {
                  
                    SecretariatDirectionProjectManager frm = new SecretariatDirectionProjectManager();
                    this.Close();
                    this.Dispose();
                    frm.ShowDialog();

                }
                else if(role.ToLower() == "Admin".ToLower())
                {
                    AdminUserManagerForm admin = new AdminUserManagerForm();
                    this.Close();
                    this.Dispose();
                    admin.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("le login ou mot de passe est incorrecte. ", "erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error
  );
                return;
            }
        }
    }
}
