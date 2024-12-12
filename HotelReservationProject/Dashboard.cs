using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelReservationProject
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.BackColor = Color.Transparent;
            
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
         
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            FrmCustomerRegistration frmCustomerRegistration = new FrmCustomerRegistration();
            frmCustomerRegistration.ShowDialog();
           
        }

      
        private void btnCustomerDetails_Click(object sender, EventArgs e)
        {
            FrmCustomerDetails customerDetailsForm = new FrmCustomerDetails();
            customerDetailsForm.ShowDialog();
            this.Hide();

        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            FrmAddRoom frmAddingRoom = new FrmAddRoom();
            frmAddingRoom.ShowDialog();
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
           FrmCheckOut frmCheckOut = new FrmCheckOut();
            frmCheckOut.ShowDialog();
           
        }

      
    }
}
