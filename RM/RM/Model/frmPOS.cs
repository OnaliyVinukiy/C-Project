using Guna.UI2.WinForms;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RM.Model;
namespace RM.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public string OrderType = "";
        public string customerName = "";
        public string customerPhone = "";
        public string customerAddress = "";

        public int DriverID = 0;

          

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
            txtcusphone.Visible = false;
            txtdrivername.Visible = false;
            lblcusname.Visible = false;
            txtCusAdd.Visible = false;

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


                    //event for click
                    b.Click += new EventHandler(b_Click);
                    CategoryPanel.Controls.Add(b);
                }
                
                
                

            }
        }
        private void AddItems(string id, string proID,string name, string cat, string price, Image pImage)
        {
            var w = new UserProduct()
            {
                pName = name,
                pPrice = price,
                pCategory = cat,
                PImage = pImage,
                id = Convert.ToInt32(proID)
            };
            productPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (UserProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    //this will check whether product is already there then add one to quantity and update price
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = (int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1);
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        GetTotal();
                        return;
                    }
                }
                //this line add new product, first 0 for sr# and second 0 for id
                guna2DataGridView1.Rows.Add(new object[] { 0,0, wdg.id, wdg.pName, 1, wdg.pPrice, wdg.pPrice });
                GetTotal();
            };

            // Add event handler for CellValueChanged event
            guna2DataGridView1.CellValueChanged += (s, e) =>
            {
                if (e.ColumnIndex == guna2DataGridView1.Columns["dgvQty"].Index)
                {
                    var qty = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvQty"].Value);
                    var pri = Convert.ToDouble(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvPrice"].Value);
                    guna2DataGridView1.Rows[e.RowIndex].Cells["dgvAmount"].Value = qty * pri;
                    GetTotal();
                }
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

                AddItems("0",item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(), item["pPrice"].ToString(), 
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

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void GetTotal()
        {
            double tot = 0;
            lbltotal1.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            lbltotal1.Text = tot.ToString("N2");
        }
        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in productPanel.Controls)
            {
                var pro = (UserProduct)item;
                pro.Visible = pro.pCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lbltotal1.Text = "00.00";
        }

        public void btnDelivery_Click(object sender, EventArgs e)
        {
            OrderType = "Delivery";
            frmAddCustomer frm = new frmAddCustomer();
            frm.MainID = MainID;
            frm.OrderType=OrderType;
            MainClass.BlurBackground(frm);
            if (frm.DriverID>0)
            {
                DriverID = frm.DriverID;
                lblcusname.Text = "Customer Name: " + frm.txtName.Text; 
                txtcusphone.Text = "Phone: " + frm.txtPhone.Text;
                txtCusAdd.Text = "Address: " + frm.txtAddress.Text;
                txtdrivername.Text= "Driver: " + frm.cbDriver.Text;
                txtcusphone.Visible = true;
                txtdrivername.Visible = true;
                lblcusname.Visible = true;
                txtCusAdd.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }

        }

        public void btnTake_Click(object sender, EventArgs e)
        {
            OrderType = "Take Away";
            txtcusphone.Visible = false;
            txtdrivername.Visible = false;
            lblcusname.Visible = false;
            txtCusAdd.Visible = false;
        }

        public void btnDine_Click(object sender, EventArgs e)
        {

            OrderType = "Dine In";
            txtcusphone.Visible = false;
            txtdrivername.Visible = false;
            lblcusname.Visible = false;
            txtCusAdd.Visible = false;
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            //save the data in database 
            string query1 = ""; //main table
            string query2 = ""; //details table

            int detailID = 0;
            if (OrderType == "")
            {
                guna2MessageDialog1.Show("Please select Order Type");
                return;
            }
            if (MainID == 0) //insert
            {
                query1 = @"Insert into tblMain Values(@aDate,@aTime,@status,@OrderType,@total,@received,@change,@DriverID,@CustName,@CustPhone,@CustAddress);
        Select SCOPE_IDENTITY()";
                //this line will get recent @id values
            }
            else //update
            {
                query1 = @"Update tblMain Set status=@status, total=@total,received=@received,change=@change where MainID=@ID";
            }

            SqlCommand cmd = new SqlCommand(query1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID); // add parameter for ID
            cmd.Parameters.AddWithValue("@aDate", DateTime.Now.Date); // remove unnecessary conversion
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@OrderType", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lbltotal1.Text));
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@DriverID", DriverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);
            cmd.Parameters.AddWithValue("@CustAddress", customerAddress);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);
                if (detailID == 0) //insert
                {
                    query2 = @"Insert into tblDetails Values (@MainID,@proID,@qty,@price,@amount)";
                }
                else//update
                {
                    query2 = @"Update tblDetails Set proID= @proID,qty=@qty,price= @price,amount= @amount where DetailID=@ID";
                }
                SqlCommand cmd2 = new SqlCommand(query2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));
                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }

                cmd2.ExecuteNonQuery();
            }

            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

        


                guna2MessageDialog1.Show("Saved Successfully");
                MainID = 0;
                detailID = 0;
            txtcusphone.Text = "";
            txtdrivername.Text = "";
            lblcusname.Text = "";
            txtCusAdd.Text = "";

        }
        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);
            if (frm.MainID>0)
            {
                id=frm.MainID;
                LoadEntries();
            }
        }
        private void LoadEntries()
        {
            string query = @"SELECT * FROM tblMain m 
            INNER JOIN tblDetails d ON m.MainID = d.MainID 
            INNER JOIN products p ON p.pID = d.proID 
            WHERE m.MainID = @id";
            SqlCommand cmd2 = new SqlCommand(query, MainClass.con);
            cmd2.Parameters.AddWithValue("@id", id);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            if (dt2.Rows[0]["OrderType"].ToString()=="Delivery")
            {
                btnDelivery.Checked = true;
            }

            else if (dt2.Rows[0]["OrderType"].ToString() == "Take Away")
            {
                btnTake.Checked = true;
            }
            else
            {
                btnDine.Checked = true;
            }


                guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt2.Rows)
            {

                string DetailID = item["DetailID"].ToString();
                string proID = item["proID"].ToString();
                string proName = item["pName"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, DetailID, proID,proName, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);
            }
            GetTotal();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (OrderType == "")
            {
                guna2MessageDialog1.Show("Please select Order Type");
                return;
            }
            frmCheckout frm = new frmCheckout(OrderType);
            frm.MainID = id;
            frm.amt = Convert.ToDouble(lbltotal1.Text);

            string total = lbltotal1.Text; // store the value of lbltotal1.Text
            lbltotal1.Text = "00.00"; // reset the value of lbltotal1.Text

            MainClass.BlurBackground(frm);
            MainID = 0;
            guna2DataGridView1.Rows.Clear();

            frm.txtBillAmount.Text = total; // set the value of txtBillAmount to the stored value

            frmCheckout checkout = new frmCheckout(OrderType);
            checkout.ShowDialog();
        }

        
        
    
    }
}

