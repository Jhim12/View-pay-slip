using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace View_pay_slip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        UserInterface userInterface = new UserInterface();
        public LoginPage()
        {
            InitializeComponent();
            userInterface.removePageControls(this);
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            Dbconnection dbconnection = new Dbconnection(); // Instantiated class

            if (!string.IsNullOrEmpty(Login_EmpUserN.Text) && !string.IsNullOrEmpty(Login_Pass.Text))
            {
                string queryString = "SELECT * FROM dbo.employee WHERE username = @username AND password = @password";

                using (SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection()))
                {
                    try
                    {
                        sqlconnection.Open();

                        using (SqlCommand command = new SqlCommand(queryString, sqlconnection))
                        {
                            command.Parameters.AddWithValue("@username", Login_EmpUserN.Text);
                            command.Parameters.AddWithValue("@id", Login_EmpUserN.Text);
                            command.Parameters.AddWithValue("@password", Login_Pass.Text);

                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                // Authentication successful
                                string passUsername = Login_EmpUserN.Text;
                                await Navigation.PushAsync(new SetDatePage(passUsername));

                                // Clear the ID and password fields
                                Login_EmpUserN.Text = string.Empty;
                                Login_Pass.Text = string.Empty;
                            }
                            else
                            {
                                // Authentication failed
                                await DisplayAlert("Ops...", "Incorrect Username or Password!", "Ok");
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        await DisplayAlert("Error", $"SQL Error: {ex.Message}", "Ok");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"An error occurred: {ex.Message}", "Ok");
                    }
                }
            }
            else
            {
                // Handle empty username or password fields
                await DisplayAlert("Ops...", "Please enter both Username and password!", "Ok");
            }
        }

        private void showPassMethod(object sender, CheckedChangedEventArgs e)
        {
            userInterface.reusePassMethod(ShowPass, Login_Pass);
        }

    }
}