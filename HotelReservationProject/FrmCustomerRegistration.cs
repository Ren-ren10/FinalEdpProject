using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelReservationProject
{
    public partial class FrmCustomerRegistration : Form
    {

        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";
        public FrmCustomerRegistration()
        {
            InitializeComponent();

        }

        private void FrmCustomerRegistration_Load(object sender, EventArgs e)
        {
            LoadRoomsData();
            LoadCustomerData();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterCustomer();
        }




        private void LoadRoomsData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Rooms";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewRooms.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading rooms: {ex.Message}");
                }
            }
        }

        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CustomerID, FullName, Gender, Age, Phone, Address, CheckIn, RoomID FROM Customers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewCustomers.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void RegisterCustomer()
        {

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                 cmbGender.SelectedIndex == -1 ||
                 string.IsNullOrWhiteSpace(txtAge.Text) ||
                 string.IsNullOrWhiteSpace(txtMobile.Text) ||
                 string.IsNullOrWhiteSpace(txtAddress.Text) ||
                 string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedRoomId;
            int age;
            long phone;

            if (!int.TryParse(txtRoomNumber.Text, out selectedRoomId))
            {
                MessageBox.Show("Room Number must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAge.Text, out age))
            {
                MessageBox.Show("Age must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(txtMobile.Text, out phone))
            {
                MessageBox.Show("Mobile Number must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullName = txtFullName.Text;
            string gender = cmbGender.Text;
            string address = txtAddress.Text;
            string checkIn = DateTime.Now.ToString("yyyy-MM-dd");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the room exists
                    string checkQuery = "SELECT RoomID FROM Rooms WHERE RoomNumber = @RoomNumber";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@RoomNumber", selectedRoomId);
                        object result = checkCmd.ExecuteScalar();

                        if (result != null)
                        {
                            int roomId = Convert.ToInt32(result);

                            // Insert customer into the Customers table
                            string insertQuery = @"
                            INSERT INTO Customers (FullName, Gender, Age, Phone, Address, CheckIn, RoomId)
                            VALUES (@FullName, @Gender, @Age, @Phone, @Address, @CheckIn, @RoomId)";

                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@FullName", fullName);
                                insertCmd.Parameters.AddWithValue("@Gender", gender);
                                insertCmd.Parameters.AddWithValue("@Age", age);
                                insertCmd.Parameters.AddWithValue("@Phone", phone);
                                insertCmd.Parameters.AddWithValue("@Address", address);
                                insertCmd.Parameters.AddWithValue("@CheckIn", checkIn);
                                insertCmd.Parameters.AddWithValue("@RoomId", roomId);
                                insertCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Customer registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadRoomsData();
                            LoadCustomerData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("The selected room does not exist.", "Room Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void ClearForm()
        {
            txtFullName.Clear();
            cmbGender.SelectedIndex = -1;
            txtMobile.Clear();
            txtAddress.Clear();
            txtRoomNumber.Clear();
            txtAge.Clear();
            txtPrice.Clear();
        }


        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridViewCustomers.SelectedRows.Count > 0)
            {
                try
                {
                    int customerId = Convert.ToInt32(dataGridViewCustomers.SelectedRows[0].Cells["CustomerID"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string deleteQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

                            using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadCustomerData();
                            LoadRoomsData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    

     private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}