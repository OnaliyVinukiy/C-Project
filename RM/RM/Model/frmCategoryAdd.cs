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
    public partial class frmCategoryAdd : Form
    {
        

        public frmCategoryAdd()
        {
            InitializeComponent();
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public int id = 0;
        public void btnSave_Click(object sender, EventArgs e)
        {
            String query = "";
            if (id == 0) //insert 
            {
                query = "Insert into category values (@Name)";
            }
            else //update
            {
                query = "Update category set catName = @Name where catID = @id";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            if (MainClass.SQL(query, ht)>0)
            {
                MessageBox.Show("Saved Successfully...");
                id = 0;
                txtName.Text = "";
                txtName.Focus();

            }

        }
    }
}
