using ADONET_Project.Customers;
using ADONET_Project.MobileDevices;
using ADONET_Project.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADONET_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void eixtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MobileDeviceView { MdiParent = this }.Show();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MobileDeviceAdd { MdiParent = this }.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void editDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MobileDeviceEdit { MdiParent = this }.Show();
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new ViewFormCustomer { MdiParent = this }.Show();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AddFormCustomer { MdiParent = this }.Show();
        }

        private void editDeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new EditFormCustomer { MdiParent = this }.Show();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewFormOrder { MdiParent = this }.Show();
        }
    }
}
