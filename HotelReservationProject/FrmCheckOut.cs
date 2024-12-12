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
    public partial class FrmCheckOut : Form
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";
        private string nameFilter;

        private delegate DataTable ExecuteQueryDelegate(string query, SqlParameter[] parameters = null);
        private delegate void ExecuteNonQueryDelegate(string query, SqlParameter[] parameters = null);

        public FrmCheckOut()
        {
            InitializeComponent();
            LoadCheckedInCustomers();
            LoadCheckedOutCustomers();
            dataGridViewCheckIn.AutoGenerateColumns = true;
            dataGridViewCheckedOutCustomer.AutoGenerateColumns = true;
        }

        private void LoadCheckedInCustomers()
        {
            string query = @"
                SELECT Customers.CustomerID, Customers.FullName, Customers.Gender, Customers.Age, Customers.Phone,  Customers.Address, Customers.RoomID
                FROM Customers
                INNER JOIN Rooms ON Customers.RoomID = Rooms.RoomID
                WHERE Customers.CheckOut IS NULL";

            ExecuteQueryDelegate executeQuery = ExecuteQuery;
            DataTable dt = executeQuery(query);
            dataGridViewCheckIn.DataSource = dt;


            dataGridViewCheckIn.Columns["CustomerID"].HeaderText = "Customer ID";
            dataGridViewCheckIn.Columns["FullName"].HeaderText = "Full Name";
            dataGridViewCheckIn.Columns["Gender"].HeaderText = "Gender";
            dataGridViewCheckIn.Columns["Age"].HeaderText = "Age";
            dataGridViewCheckIn.Columns["Phone"].HeaderText = "Phone";
            dataGridViewCheckIn.Columns["Address"].HeaderText = "Address";
            dataGridViewCheckIn.Columns["RoomID"].HeaderText = "Room ID";
            dataGridViewCheckIn.Columns["CustomerID"].Visible = true;
        }


        private void LoadCheckedOutCustomers()
        {
            string query = @"
                SELECT Customers.CustomerID, Customers.FullName, Customers.RoomID, Rooms.RoomNumber, Customers.CheckOut
                FROM Customers
                INNER JOIN Rooms ON Customers.RoomID = Rooms.RoomID
                WHERE Customers.CheckOut IS NOT NULL";

            ExecuteQueryDelegate executeQuery = ExecuteQuery;
            DataTable dt = executeQuery(query);
            dataGridViewCheckedOutCustomer.DataSource = dt;

            dataGridViewCheckedOutCustomer.Columns["CustomerID"].HeaderText = "Customer ID";
            dataGridViewCheckedOutCustomer.Columns["FullName"].HeaderText = "Full Name";
            dataGridViewCheckedOutCustomer.Columns["RoomID"].HeaderText = "Room ID";
            dataGridViewCheckedOutCustomer.Columns["RoomNumber"].HeaderText = "Room Number";
            dataGridViewCheckedOutCustomer.Columns["CheckOut"].HeaderText = "Check Out Time";
        }


        private DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SQL Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            string nameFilter = txtSearchName.Text.Trim();

            string query = @"
                SELECT Customers.CustomerID, Customers.FullName, Customers.RoomID, Rooms.RoomNumber
                FROM Customers
                INNER JOIN Rooms ON Customers.RoomID = Rooms.RoomID
                WHERE Customers.CheckOut IS NULL";

            if (!string.IsNullOrEmpty(nameFilter))
            {
                query += " AND Customers.FullName LIKE @NameFilter";
            }

            ExecuteQueryDelegate executeQuery = ExecuteQuery;
            SqlParameter[] parameters = !string.IsNullOrEmpty(nameFilter) ? new[] { new SqlParameter("@NameFilter", "%" + nameFilter + "%") } : null;

            DataTable dt = executeQuery(query, parameters);
            dataGridViewCheckIn.DataSource = dt;
        }


        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (dataGridViewCheckIn.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to check out.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int customerId = Convert.ToInt32(dataGridViewCheckIn.SelectedRows[0].Cells["CustomerID"].Value);
                int roomId = Convert.ToInt32(dataGridViewCheckIn.SelectedRows[0].Cells["RoomID"].Value);
                DateTime selectedCheckOutDate = dtpCheckOut.Value;

                ExecuteNonQueryDelegate executeNonQuery = ExecuteNonQuery;

                string updateCustomerQuery = "UPDATE Customers SET CheckOut = @CheckOut WHERE CustomerID = @CustomerID";
                executeNonQuery(updateCustomerQuery, new[]
                {
                    new SqlParameter("@CheckOut", selectedCheckOutDate),
                    new SqlParameter("@CustomerID", customerId)
                });

                string updateRoomQuery = "UPDATE Rooms SET Status = 'Available' WHERE RoomID = @RoomID";
                executeNonQuery(updateRoomQuery, new[]
                {
                    new SqlParameter("@RoomID", roomId)
                });

                LoadCheckedInCustomers();
                LoadCheckedOutCustomers();

                MessageBox.Show("Customer checked out successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during checkout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        


        private void FrmCheckOut_Load(object sender, EventArgs e)
        {
            dtpCheckOut.Value = DateTime.Now;
            LoadCheckedInCustomers();
            LoadCheckedOutCustomers();
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
