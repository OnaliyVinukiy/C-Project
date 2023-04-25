﻿using System;
using System.Collections;
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
using System.Xml.Linq;

namespace RM.Model
{
    public partial class frmProductAdd : Form
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }
        public int id = 0;
        public int cID = 9;


        string filePath;
        byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (.png,.jpg)|* .png; * .jpg";
            if(ofd.ShowDialog() == DialogResult.OK ) 
            { 
                filePath = ofd.FileName;
                txtImage.Image= new Bitmap(filePath);
            }
        }
        
        

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            //for cb fill
            string query = "Select catID 'id',catName 'name' from category ";
            MainClass.CBFill(query, cbCat);
            if (cID>0) //for update
            {
                cbCat.SelectedValue = cID;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String query = "";
            if (id == 0) //insert 
            {
                query = "Insert into products values (@Name, @price, @cat,@img)";
            }
            else //update
            {
                query = "Update products set pName = @Name, pPrice= @price,CategoryID= @cat,pImage=@img where pID = @id";
            }

            //for image
            Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@price", txtPrice.Text);
            ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@img", imageByteArray);
            if (MainClass.SQL(query, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved Successfully...");
                id = 0;
                txtName.Text = "";
                txtPrice.Text = "";
                cbCat.SelectedIndex = -1;
                txtImage.Image = RM.Properties.Resources.icons8_food_bar_1000;
                txtName.Focus();

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ForUpdateLoadData()
        {
            string query = @"Select * from products where pID=" + id + "";
            SqlCommand cmd = new SqlCommand(query, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
        }
    }
}
