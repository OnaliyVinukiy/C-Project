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
using System.Windows.Forms.VisualStyles;



namespace RM.Model
{
    public partial class frmCheckout : Form
    {
        private frmPOS formPOS;

        public frmCheckout(string orderType)
        {
            InitializeComponent();
            OrderType = orderType;
        }

       

        public double amt;
        public int MainID = 0;
        public string OrderType;
       
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double receipt = 0;
            double change = 0;
            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtReceived2.Text, out receipt);


            change = Math.Abs(amt - receipt);
            txtChange.Text = change.ToString();
        }

        

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString();

        }

       
            public virtual void btnnSave_Click(object sender, EventArgs e)
        {
            string query = "";
            if (MainID == 0)
            {
                query = @"Insert into tblMain (aDate,aTime,status,OrderType,total,received,change) values (@date,@time,'Paid',@ordertype,@total,@rec,@change)";
            }
            else
            {
                query = @"Update tblMain set status='Paid', total=@total, received=@rec, change=@change where MainID = @id";
            }
            frmPOS form1 = new frmPOS();
            Hashtable ht = new Hashtable();
            ht.Add("@id", MainID);
            ht.Add("@total", txtBillAmount.Text);
            ht.Add("@rec", txtReceived2.Text);
            ht.Add("@change", txtChange.Text);
            ht.Add("@date", DateTime.Now.Date);
            ht.Add("@time", DateTime.Now.ToShortTimeString());
            ht.Add("@ordertype", OrderType);

            if (MainClass.SQL(query, ht) > 0)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Saved Successfully");

                this.Close();
            }
        }
    }



    }

