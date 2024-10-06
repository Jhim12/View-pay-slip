using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace View_pay_slip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeCredentialsPage : ContentPage
    {
        PayslipAandM payslip = new PayslipAandM();
        UserInterface userInterface = new UserInterface();
        public ChangeCredentialsPage(string passID)
        {
            InitializeComponent();

            payslip.id = Convert.ToInt32(passID);
        }

        private async void ChangeUserN_Clicked(object sender, EventArgs e)
        {
            Dbconnection dbconnection = new Dbconnection();
            try
            {
                payslip.CurrentUsername = CurUserN.Text;
                payslip.NewUsername = NewUserN.Text;

                // Checks if text boxes are empty
                if (string.IsNullOrWhiteSpace(payslip.CurrentUsername))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The current username field is empty.", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(payslip.NewUsername))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The new username field is empty.", "OK");
                    return;
                }
                else if (payslip.NewUsername == payslip.CurrentUsername)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "New username cannot be the same as the current username.", "OK");
                    return;
                }

                // Display a confirmation prompt
                bool confirmedUserName = await DisplayAlert("Confirmation", "Do you want to change your username?", "Yes", "No");

                if (!confirmedUserName)
                {
                    return;
                }

                // sql connection
                SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection()); 
                sqlconnection.Open();

                // Verify the current username
                string userNameQryStr = "SELECT id, username FROM dbo.employee WHERE id = @id AND username = @username";
                using (SqlCommand command = new SqlCommand(userNameQryStr, sqlconnection))
                {
                    command.Parameters.AddWithValue("@id", payslip.id);
                    command.Parameters.AddWithValue("@username", payslip.CurrentUsername);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Close(); 

                            // Check if an username is already exist
                            using (SqlCommand checkUserNameExist = new SqlCommand("SELECT COUNT(*) FROM dbo.employee WHERE username = @newusername", sqlconnection))
                            {
                                checkUserNameExist.Parameters.AddWithValue("@newusername", payslip.NewUsername);

                                int countUsernameExist = (int)checkUserNameExist.ExecuteScalar();

                                if (countUsernameExist > 0)
                                {
                                    await App.Current.MainPage.DisplayAlert("Already exist!", "The new username you put is already exist.", "Ok");
                                    return; // Exit the method without inserting the duplicate record
                                }
                            }

                            string updateUsernameQryStr = "UPDATE dbo.employee SET username = @newusername WHERE id = @id";
                            using (SqlCommand updateUsername = new SqlCommand(updateUsernameQryStr, sqlconnection))
                            {
                                updateUsername.Parameters.AddWithValue("@newusername", payslip.NewUsername);
                                updateUsername.Parameters.AddWithValue("@id", payslip.id);

                                int rowsAffected = updateUsername.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Show success message
                                    await Application.Current.MainPage.DisplayAlert("Success", "Username changed successfully.", "OK");

                                    // Reset password fields
                                    CurUserN.Text = "";
                                    NewUserN.Text = "";

                                }
                                else
                                {
                                    // Show error message if the update failed
                                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to change username.", "OK");
                                }
                            }
                        }
                        else
                        {
                            // Show error message if current username is incorrect
                            await Application.Current.MainPage.DisplayAlert("Error", "Current username is incorrect.", "OK");
                        }
                    }
                }
                
            }

            catch (Exception ex)
            {
                // Handle exceptions and show error message
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private async void ChangePass_Clicked(object sender, EventArgs e)
        {
            // sql connection
            Dbconnection dbconnection = new Dbconnection();
            try
            {
                payslip.CurrentPassword = CurPass.Text;
                payslip.NewPassword = NewPass.Text;
                payslip.ConfirmPassword = ConPass.Text;

                // checks if empty
                if (string.IsNullOrWhiteSpace(payslip.CurrentPassword))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The current password field is empty.", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(payslip.NewPassword))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The new password field is empty.", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(payslip.ConfirmPassword))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The confirm password field is empty.", "OK");
                    return;
                }

                // 
                if (payslip.NewPassword != payslip.ConfirmPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "New password and confirm password do not match.", "OK");
                    return;
                }
                else if (payslip.NewPassword == payslip.CurrentPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "New password cannot be the same as the current password.", "OK");
                    return;
                }

                // Display a confirmation prompt
                bool result = await DisplayAlert("Confirmation", "Do you want to change your password?", "Yes", "No");

                if (!result)
                {
                    // User chose "No," so cancel the operation
                    return;
                }

                SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection());
                sqlconnection.Open();

                // Verify the current password
                string query = "SELECT * FROM dbo.employee WHERE id = @id AND password = @password";
                using (SqlCommand command = new SqlCommand(query, sqlconnection))
                {
                    command.Parameters.AddWithValue("@id", payslip.id);
                    command.Parameters.AddWithValue("@password", payslip.CurrentPassword);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Update the password if the current password is correct
                            reader.Close(); // Close the reader before executing the update query

                            string updateQuery = "UPDATE dbo.employee SET password = @newPassword WHERE id = @id AND password = @password";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlconnection))
                            {
                                updateCommand.Parameters.AddWithValue("@id", payslip.id); // to avoid change the same password to the other users
                                updateCommand.Parameters.AddWithValue("@newPassword", payslip.NewPassword);
                                updateCommand.Parameters.AddWithValue("@password", payslip.CurrentPassword);

                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Show success message
                                    await Application.Current.MainPage.DisplayAlert("Success", "Password changed successfully.", "OK");

                                    // Reset password fields
                                    CurPass.Text = "";
                                    NewPass.Text = "";
                                    ConPass.Text = "";

                                    // Uncheck the checkboxes after successful password change
                                    ShowCurPass.IsChecked = false;
                                    ShowNewPass.IsChecked = false;
                                    ShowConPass.IsChecked = false;

                                    // Hide the password text if checkboxes are unchecked
                                    CurPass.IsPassword = true;
                                    NewPass.IsPassword = true;
                                    ConPass.IsPassword = true;

                                    await Navigation.PushAsync(new LoginPage());

                                }
                                else
                                {
                                    // Show error message if the update failed
                                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to change password.", "OK");
                                }
                            }
                        }
                        else
                        {
                            // Show error message if current password is incorrect
                            await Application.Current.MainPage.DisplayAlert("Error", "Current password is incorrect.", "OK");
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and show error message
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ShowCurPass_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            userInterface.reusePassMethod(ShowCurPass, CurPass);
        }

        private void ShowNewPass_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            userInterface.reusePassMethod(ShowNewPass, NewPass);
        }

        private void ShowConPass_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            userInterface.reusePassMethod(ShowConPass, ConPass);
        }

    }
}