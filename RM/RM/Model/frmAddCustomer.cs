using System;
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
    public partial class frmAddCustomer : Form
    {
        public frmAddCustomer()
        {
            InitializeComponent();
        }

        public string OrderType = "";
        public int DriverID = 0;
       
        public int MainID = 0;

        private void frmAddCustomer_Load(object sender, EventArgs e)
        {
            
            string query="Select staffID 'id', sName 'name' from staff where sRole='Driver'";
            MainClass.CBFill(query, cbDriver);
            if (MainID>0)
            {
                cbDriver.SelectedValue = DriverID;
            }
        }

        private void cbDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriverID=Convert.ToInt32(cbDriver.SelectedValue);
        }
    }
}
