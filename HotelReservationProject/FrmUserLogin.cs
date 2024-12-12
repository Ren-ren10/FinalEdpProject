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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HotelReservationProject
{
    public partial class FrmUserLogin : Form
    {

        public delegate bool ValidateCredentials(string username, string password);
        private ValidateCredentials validateCredentialsHandler;


        public FrmUserLogin(ValidateCredentials validateHandler)
        {
            InitializeComponent();
            this.validateCredentialsHandler = validateHandler;
        }


        public FrmUserLogin()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ADMIN\\source\\repos\\HotelReservationProject\\HotelReservationProject\\myDB.mdf;Integrated Security=True";


        
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
           
        }

    
        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Userpass = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1) 
                    {
                        lblErrorMessage.Hide(); 
                        FrmDashboard dashBoard = new FrmDashboard();
                        dashBoard.ShowDialog();
                        this.Hide();
                    }
                    else 
                    {
                        lblErrorMessage.Text = "Incorrect username or password.";
                        lblErrorMessage.ForeColor = Color.Red; 
                        lblErrorMessage.Show();

                        txtUsername.Clear();
                        txtPassword.Clear();
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Error during login: {ex.Message}");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
