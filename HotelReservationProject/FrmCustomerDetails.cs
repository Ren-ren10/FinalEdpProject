using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelReservationProject
{
    public partial class FrmCustomerDetails : Form
    {

        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";
        public FrmCustomerDetails()
        {
            InitializeComponent();
            LoadCustomerDetails();
        }

        private void LoadCustomerDetails(string nameFilter = "")
        {
            string query = @"
                SELECT CustomerID, FullName, Gender, Age, Phone, Address, CheckIn, CheckOut, RoomID
                FROM Customers";

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query += " WHERE FullName LIKE @NameFilter";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                    if (!string.IsNullOrEmpty(nameFilter))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@NameFilter", "%" + nameFilter + "%");
                    }

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    dataGridViewCustomerDetails.AutoGenerateColumns = false;
                    dataGridViewCustomerDetails.Columns.Clear();

                 
                    AddColumn("CustomerID", "Customer ID", "CustomerID", dt);
                    AddColumn("FullName", "Full Name", "FullName", dt);
                    AddColumn("Gender", "Gender", "Gender", dt);
                    AddColumn("Age", "Age", "Age", dt);
                    AddColumn("Phone", "Phone", "Phone", dt);
                    AddColumn("Address", "Address", "Address", dt);
                    AddColumn("CheckIn", "Check-In Date", "CheckIn", dt);
                    AddColumn("CheckOut", "Check-Out Date", "CheckOut", dt);
                    AddColumn("RoomID", "Room Number", "RoomID", dt);

                    
                    dataGridViewCustomerDetails.DataSource = dt;

                    
                    dataGridViewCustomerDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    dataGridViewCustomerDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                    dataGridViewCustomerDetails.EnableHeadersVisualStyles = false;
                    dataGridViewCustomerDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddColumn(string name, string headerText, string dataPropertyName, DataTable dt)
        {
            if (!dt.Columns.Contains(dataPropertyName))
            {
                MessageBox.Show($"Column '{dataPropertyName}' not found in data source.", "Column Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = dataPropertyName,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dataGridViewCustomerDetails.Columns.Add(column);
        }




        private void btnCDSearch_Click(object sender, EventArgs e)
        {
            string nameFilter = txtsearchName.Text.Trim();
            LoadCustomerDetails(nameFilter); 
        }

        
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtsearchName.Clear();
            LoadCustomerDetails(); 
        }

        private void FrmCustomerDetails_Load(object sender, EventArgs e)
        {
            LoadCustomerDetails(); 
        }

        private void FrmCustomerDetails_Load_1(object sender, EventArgs e)
        {
            LoadCustomerDetails();
        }

        private void btnClearSearch_Click_1(object sender, EventArgs e)
        {
            txtsearchName.Clear();
            LoadCustomerDetails();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      
    }

}
    