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
    public partial class FrmUpdateRoom : Form
    {

        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";

        private string roomNumber;
        private string roomType;
        private decimal price;
        private string bed;

        public delegate bool RoomUpdateHandler(string roomNumber, string roomType, string bed, decimal price);
        private RoomUpdateHandler updateHandler;
        public FrmUpdateRoom(string roomNumber, string roomType, decimal price, string bed, RoomUpdateHandler handler)
        {
            InitializeComponent();
            txtRoomNumber.Text = roomNumber;
            cmbRoomType.Text = roomType;
            txtPrice.Text = price.ToString();
            cmbBed.Text = bed;

      
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                string roomNumber = txtRoomNumber.Text;
                string roomType = cmbRoomType.Text;
                decimal price = Convert.ToDecimal(txtPrice.Text);
                string bed = cmbBed.Text;

               
                if (UpdateRoomInDatabase(roomNumber, roomType, bed, price))
                {
                    MessageBox.Show("Room updated successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update room. Please check your inputs.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating room: {ex.Message}");
            }
        }

        private bool UpdateRoomInDatabase(string roomNumber, string roomType, string bed, decimal price)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Rooms SET RoomType = @RoomType, Price = @Price, Bed = @Bed WHERE RoomNumber = @RoomNumber";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        cmd.Parameters.AddWithValue("@RoomType", roomType);
                        cmd.Parameters.AddWithValue("@Bed", bed);
                        cmd.Parameters.AddWithValue("@Price", price);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating room in database: {ex.Message}");
                return false;
            }
        }
    
    



private void FrmEditRoom_Load(object sender, EventArgs e)
        {

        }
    }
}
