using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADONET_Project.Orders
{
    public partial class OrderItemAdd : Form
    {
        public OrderItemAdd()
        {
            InitializeComponent();
        }
        public ViewFormOrder MainFormOrder { get; set; }
        public int OrderId { get; set; }
        private void OrderItemAdd_Load(object sender, EventArgs e)
        {
            LoadComboBox();
        }
        private void LoadComboBox()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM MobileDevices ", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    this.comboBox1.DataSource = dt;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Price FROM MobileDevices WHERE MobileDeviceId=@i", con))
                {
                    cmd.Parameters.AddWithValue("@i", comboBox1.SelectedValue); 
                    con.Open();
                    decimal p = (decimal)cmd.ExecuteScalar();
                    con.Close();
                    this.lblPrice.Text = p.ToString("0:00");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {

                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO OrderItems 
                                            (OrderId, MobileDeviceId, Quantity) VALUES
                                            (@o, @m, @q)", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@o", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@m", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@q", textBox1.Text);


                        try
                        {
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                tran.Commit();
                                this.textBox1.Clear();
                              
                                con.Close();
                             
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tran.Rollback();
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                            {
                                con.Close();
                            }
                        }

                    }
                }

            }
        }

        private void OrderItemAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainFormOrder.LoadData(); 
        }
    }
}
