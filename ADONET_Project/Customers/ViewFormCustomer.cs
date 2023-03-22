using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADONET_Project.Customers
{
    public partial class ViewFormCustomer : Form
    {
        public ViewFormCustomer()
        {
            InitializeComponent();
        }

        private void CustomerView_Load(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    this.dataGridView1.DataSource = dt;
                    
                }
            }
        }
    }
}
