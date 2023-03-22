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

namespace ADONET_Project.MobileDevices
{
    
    public partial class MobileDeviceEdit : Form
    {
        string pictureName = "", oldPictrureName = "";
        public MobileDeviceEdit()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM MobileDevices WHERE MobileDeviceId=@i", con))
                {
                    cmd.Parameters.AddWithValue("@i", comboBox1.SelectedValue);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox2.Text = dr.GetString(1);
                        textBox3.Text = dr.GetDecimal(2).ToString("0:00");
                        textBox4.Text = dr.GetInt32(3).ToString();
                        dateTimePicker1.Value = dr.GetDateTime(4);
                        pictureBox1.Image = Image.FromFile(Path.Combine(@"..\..\Pictures", dr.GetString(5).ToString()));
                        textBox5.Text = dr.GetString(6);
                        oldPictrureName = dr.GetString(5);
                    }
                    con.Close();
                }
            }
        }

        private void MobileDeviceEdit_Load(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(@"UPDATE MobileDevices 
                                            SET MobileDeviceName=@n, Price=@p, Quantity=@q, RelaeseDate=@r, Picture=@pic, CountryOfOrigin=@c WHERE
                                            MobileDeviceId =@i", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@i", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@n", textBox2.Text);
                        cmd.Parameters.AddWithValue("@p", textBox3.Text);
                        cmd.Parameters.AddWithValue("@q", textBox4.Text);
                        cmd.Parameters.AddWithValue("@r", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@c", textBox5.Text);
                        if (!string.IsNullOrEmpty(pictureName))
                        {
                            string ext = Path.GetExtension(this.pictureName);
                            string fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{ext}";
                            string savePath = Path.Combine(Path.GetFullPath(@"..\..\Pictures"), fileName);
                            File.Copy(pictureName, savePath, true);
                            cmd.Parameters.AddWithValue("@pic", fileName);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@pic", oldPictrureName);
                        }
                        try
                        {
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                tran.Commit();

                                pictureName = "";
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
                        con.Close();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are your sure to delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE MobileDevices WHERE MobileDeviceId=@i", con))
                    {
                        cmd.Parameters.AddWithValue("@i", comboBox1.SelectedValue);
                        con.Open();
                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadComboBox();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Data delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            if (con.State == ConnectionState.Open)
                            {
                                con.Open();
                            }
                        }

                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                this.pictureName = this.openFileDialog1.FileName;
            }
        }
    }
}
