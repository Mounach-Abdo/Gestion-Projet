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
                    where += " AND " + CheckNom.Tag + "='" + textNom.Text + "'";
                if ( CheckOrganisme.Checked )
                    where += " AND " + CheckOrganisme.Tag + " = '" + textOrganismeID.Text + "'";
                var data = Database.GetdDataFromDatabase(sql + where + orderBy);

                gridResults.DataSource = data;
                gridResults.Update();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
