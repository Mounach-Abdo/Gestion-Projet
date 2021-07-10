using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTION_DE_PROJET.Shared.Recherche
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }

        public string sql;
        public string where;
        public string typeRecherche;
        public string title;
        public List<string> columns;
        public string orderBy;
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var where = " Where 0=0 ";

                if ( checkCode.Checked )
                {
                    where += " AND " + checkCode.Tag + "='" + textCode.Text + "'";
                }

                if ( CheckNom.Checked )
                    where += " AND " + CheckNom.Tag + " LIKE '%" + textNom.Text + "%'";
                if ( CheckOrganisme.Checked )
                    where += " AND " + CheckOrganisme.Tag + " LIKE '%" + textOrganismeID.Text + "%'";

                var data = Database.GetdDataFromDatabase(sql + where + orderBy);

                gridResults.DataSource = data;
                gridResults.Update();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkCode_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if ( !checkCode.Checked )
                textCode.Text = "";
        }

        private void textCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckNom_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if ( !CheckNom.Checked )
                textNom.Text = "";
        }

        private void CheckOrganisme_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if ( !CheckOrganisme.Checked )
                textOrganismeID.Text = "";
        }
    }
}
