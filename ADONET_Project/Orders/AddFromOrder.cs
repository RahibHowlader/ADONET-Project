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
    public partial class AddFromOrder : Form
    {
        public AddFromOrder()
        {
            InitializeComponent();
        }
        public ViewFormOrder MainFormOrder { get; set; }

        private void AddFromOrder_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = GetOrderId().ToString();
            LoadComboBox();
        }
        private int GetOrderId()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(OrderId), 0) FROM Orders", con))
                {
                    con.Open();
                    int id = (int)cmd.ExecuteScalar();
                    con.Close();
                    return id + 1;
                }
            }
        }
        private void LoadComboBox()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers ", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    this.comboBox1.DataSource = dt;
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

                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Orders 
                                            (OrderId, OrderDate, CustomerId) VALUES
                                            (@i, @o, @c)", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@i", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@o", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@c", comboBox1.SelectedValue);
                        

                        try
                        {
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                tran.Commit();
                                this.textBox1.Clear();
                                dateTimePicker1.Value = DateTime.Now;
                                con.Close();
                                this.textBox1.Text = GetOrderId().ToString();
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

        private void AddFromOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.MainFormOrder.LoadData();
        }
    }
}
