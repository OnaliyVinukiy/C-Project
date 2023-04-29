using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmStaffAdd : Form
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }
        public int id = 0;

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }
        

        public void btnSave_Click_1(object sender, EventArgs e)
        {
            String query = "";
            if (id == 0) //insert 
            {
                query = "Insert into staff values (@Name, @Phone, @Role)";
            }
            else //update
            {
                query = "Update staff set sName = @Name, sPhone= @phone,sRole= @role where staffID = @id";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@Phone", txtPhone.Text);
            ht.Add("@Role", cbRole.Text);
            if (MainClass.SQL(query, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved Successfully...");
                id = 0;
                txtName.Text = "";
                txtPhone.Text = "";
                cbRole.SelectedIndex = -1;
                txtName.Focus();
                this.Close();

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        { 
                this.Close();
            
        }
    }
}
