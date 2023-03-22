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
    public partial class MobileDeviceAdd : Form
    {
        string pictureName = "";
        public MobileDeviceAdd()
        {
            InitializeComponent();
        }

        private void MobileDeviceAdd_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = GetDeviceId().ToString();
        }
        private int GetDeviceId()
        {
            using (SqlConnection con = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(MobileDeviceId), 0) FROM MobileDevices", con))
                {
                    con.Open();
                    int id = (int)cmd.ExecuteScalar();
                    con.Close();
                    return id + 1;
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

                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO MobileDevices 
                                            (MobileDeviceId, MobileDeviceName, Price, Quantity, RelaeseDate, Picture, CountryOfOrigin) VALUES
                                            (@i, @n, @p, @q, @r, @pic, @c)", con, tran))
                    {
                        cmd.Parameters.AddWithValue("@i", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@n", textBox2.Text);
                        cmd.Parameters.AddWithValue("@p", decimal.Parse(textBox3.Text));
                        cmd.Parameters.AddWithValue("@q", textBox4.Text);
                        cmd.Parameters.AddWithValue("@r", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@c", textBox5.Text);
                        string ext = Path.GetExtension(this.pictureName);
                        string fileName = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{ext}";
                        string savePath = Path.Combine(Path.GetFullPath(@"..\..\Pictures"), fileName);
                        File.Copy(pictureName, savePath, true);
                        cmd.Parameters.AddWithValue("@pic", fileName);

                        try
                        {
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("Data Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                tran.Commit();
                                this.textBox1.Clear();
                                this.textBox2.Clear();
                                this.textBox3.Clear();
                                dateTimePicker1.Value = DateTime.Now;
                                this.textBox4.Clear();
                                this.textBox5.Clear();
                                pictureBox1.Image = null;
                                pictureName = "";
                                con.Close();
                                this.textBox1.Text = GetDeviceId().ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                this.pictureName = this.openFileDialog1.FileName;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
