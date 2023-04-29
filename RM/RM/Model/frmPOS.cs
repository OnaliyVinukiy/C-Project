using Guna.UI2.WinForms;
using Microsoft.VisualBasic;
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

namespace RM.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();
            productPanel.Controls.Clear();
            LoadProducts();

        }
        private void AddCategory()
        {
            string query = "Select * from Category";
            SqlCommand cmd = new SqlCommand(query,MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CategoryPanel.Controls.Clear();
            if (dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(0, 128, 128);
                    b.Size = new Size(201, 62);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();
                    CategoryPanel.Controls.Add(b);
                }
                
                
                

            }
        }
        private void AddItems (int id,string name,string cat,string price, Image pImage)
        {
            var w = new UserProduct()
            {
                pName = name,
                pPrice = price,
                pCategory = cat,
                PImage = pImage,
                id = Convert.ToInt32(id)
            };
            productPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (UserProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    //this will check whether product is already there then add one to quantity and update price
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = (int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1);
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());

                    }
                    
              
                }
                //this line add new product
                guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.pName, 1, wdg.pPrice, wdg.pPrice });
            };
        }
        //getting product from database
        private void LoadProducts ()
        {
            string query = "select * from products inner join category on catID = CategoryID";
            SqlCommand cmd = new SqlCommand(query, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imagebytearray = imagearray;

                AddItems(int.Parse(item["pID"].ToString()), item["pName"].ToString(), item["catName"].ToString(), item["pPrice"].ToString(), 
                    Image.FromStream(new MemoryStream(imagearray)));
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in productPanel.Controls)
            {
                var pro =(UserProduct)item;
                pro.Visible = pro.pName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }
    }
}
