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
    public partial class FrmAddRoom : Form
    {

        public delegate bool RoomUpdateDelegate(string roomNumber, string roomType, string bed, decimal price);

        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";
        private string roomNumber;
        private string roomType;
        private decimal price;
        private string bed;
        private RoomUpdateDelegate updateDelegate;

        public FrmAddRoom(RoomUpdateDelegate updateDelegate)
        {
            InitializeComponent();
            this.updateDelegate = updateDelegate;
        }

        public FrmAddRoom()
        {
            InitializeComponent();
        }




        public FrmAddRoom(string roomNumber, string roomType, decimal price, string bed, RoomUpdateDelegate updateDelegate)
        {
            InitializeComponent();
            this.roomNumber = roomNumber;
            this.roomType = roomType;
            this.price = price;
            this.bed = bed;
            this.updateDelegate = updateDelegate;

        
            txtRoomNo.Text = roomNumber;
            cbRoomType.Text = roomType;
            txtPrice.Text = price.ToString();
            cbBed.Text = bed;


        }


        private void LoadRoomData()
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
                    dataGridRoom.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }

        private void FrmAddingRoom_Load(object sender, EventArgs e)
        {
            LoadRoomData();
        }

        private bool UpdateRoomInDatabase(string roomNumber, string roomType, string bed, decimal price)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Rooms SET RoomType = @RoomType, Price = @Price, Bed = @Bed WHERE RoomNumber = @RoomNumber";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    cmd.Parameters.AddWithValue("@RoomType", roomType);
                    cmd.Parameters.AddWithValue("@Bed", bed);
                    cmd.Parameters.AddWithValue("@Price", price);

                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating room: {ex.Message}");
                    return false;
                }
            }
        }

        private void dataGridRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridRoom.Rows[e.RowIndex].Cells.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dataGridRoom.Rows[e.RowIndex];

                    string roomNumber = row.Cells["RoomNumber"].Value?.ToString() ?? string.Empty;
                    string roomType = row.Cells["RoomType"].Value?.ToString() ?? string.Empty;
                    string bed = row.Cells["Bed"].Value?.ToString() ?? string.Empty;

                    decimal price = 0;
                    if (row.Cells["Price"].Value != DBNull.Value && decimal.TryParse(row.Cells["Price"].Value?.ToString(), out decimal parsedPrice))
                    {
                        price = parsedPrice;
                    }

                    FrmUpdateRoom editForm = new FrmUpdateRoom(roomNumber, roomType, price, bed, UpdateRoomInDatabase);
                    editForm.ShowDialog();

                    LoadRoomData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting or editing the row: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a valid room row.");
            }
        }
    

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Rooms (RoomNumber, RoomType, Bed, Price, Status) VALUES (@RoomNumber, @RoomType, @Bed, @Price, @Status)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@RoomNumber", txtRoomNo.Text);
                    cmd.Parameters.AddWithValue("@RoomType", cbRoomType.Text);
                    cmd.Parameters.AddWithValue("@Bed", cbBed.Text);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@Status", txtStatus.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Room added successfully!");
                        LoadRoomData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding room: {ex.Message}");
                }
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dataGridRoom.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a room to edit.");
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataGridRoom.SelectedRows[0];

                string roomNumber = selectedRow.Cells["RoomNumber"].Value?.ToString() ?? string.Empty;
                string roomType = selectedRow.Cells["RoomType"].Value?.ToString() ?? string.Empty;
                string bed = selectedRow.Cells["Bed"].Value?.ToString() ?? string.Empty;

                decimal price = 0;
                if (selectedRow.Cells["Price"].Value != DBNull.Value &&
                    decimal.TryParse(selectedRow.Cells["Price"].Value?.ToString(), out decimal parsedPrice))
                {
                    price = parsedPrice;
                }

                FrmUpdateRoom editForm = new FrmUpdateRoom(roomNumber, roomType, price, bed, UpdateRoomInDatabase);
                editForm.ShowDialog();
                LoadRoomData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing room: {ex.Message}");
            }
        }
    }
}
