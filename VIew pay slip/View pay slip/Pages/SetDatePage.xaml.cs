using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace View_pay_slip.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetDatePage : ContentPage
    {

        PayslipAandM payslip = new PayslipAandM();
        UserInterface userInterface = new UserInterface();
        public SetDatePage(string passUsername) // contain the value of the prev page
        {
            InitializeComponent();
            // sql connection
            Dbconnection dbconnection = new Dbconnection();
            SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection());

            userInterface.removePageControls(this);

            PopulateYears();

            // Display Personal information of the user and the date
            try
            {
                sqlconnection.Open();

                List<PayslipAandM> empInfoList = new List<PayslipAandM>();
                string empInfoQryStr = $"SELECT id, firstname, mname, surname FROM dbo.employee WHERE username = @username";
                SqlCommand command = new SqlCommand(empInfoQryStr, sqlconnection);
                command.Parameters.AddWithValue("@username", passUsername);
                SqlDataReader reader = command.ExecuteReader();

                // Clear existing result and add new ones
                resultPicker.Items.Clear();

                while (reader.Read())
                {
                    // In this part you must put these columns in the query
                    payslip.id = reader["id"] is DBNull ? 0 : Convert.ToInt32(reader["id"]);
                    payslip.firstname = reader["firstname"] is DBNull ? string.Empty : reader["firstname"].ToString();
                    payslip.mname = reader["mname"] is DBNull ? string.Empty : reader["mname"].ToString();
                    payslip.surname = reader["surname"] is DBNull ? string.Empty : reader["surname"].ToString();

                    PayslipAandM empInfo = new PayslipAandM
                    {
                        id = payslip.id,
                        firstname = payslip.firstname
                    };
                    empInfoList.Add(empInfo);
                }
                reader.Close(); // close reader

                sqlconnection.Close();

                // Add the current date to the data model
                string formattedDate = DateTime.Now.ToString("dddd, MMMM d, yyyy");

                foreach (var empInfo in empInfoList)
                {
                    empInfo.currentDate = formattedDate;
                }

                if (empInfoList.Count > 0)
                {
                    EmpInfoList.ItemsSource = empInfoList;
                }
            }

            catch (Exception ex)
            {

                throw;
            }

            // Attach the event handler to both monthPicker and yearPicker
            monthPicker.SelectedIndexChanged += OnPickerSelectionChanged;
            yearPicker.SelectedIndexChanged += OnPickerSelectionChanged;

        }

        // Define a single event handler for both pickers
        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            // Call PaySlipNo() when either picker selection changes
            PaySlipNo();
        }

        private void PaySlipNo()
        {
            // sql connection
            Dbconnection dbconnection = new Dbconnection();

            // Check if both month and year have value
            if (monthPicker.SelectedItem != null && yearPicker.SelectedItem != null)
            {

                try
                {
                    SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection());
                    sqlconnection.Open();

                    string empPayslipNoQryStr = $"SELECT *, DAY(dtefrom) AS DayFrom, DAY(dteto) AS DayTo" +
                    $" FROM payroll" +
                    $" WHERE empid = @empid" +
                    $" AND (DATEPART(month, dtefrom) = @month)" +
                    $" AND (DATEPART(year, dtefrom) = @year)";
                    SqlCommand payslipNoCommand = new SqlCommand(empPayslipNoQryStr, sqlconnection);
                    payslipNoCommand.Parameters.AddWithValue("@empid", payslip.id);
                    payslipNoCommand.Parameters.AddWithValue("@month", payslip.getMonthNumber(monthPicker.SelectedItem));
                    payslipNoCommand.Parameters.AddWithValue("@year", yearPicker.SelectedItem.ToString());
                    SqlDataReader payslipNoReader = payslipNoCommand.ExecuteReader();

                    // Clear existing result before adding new ones
                    resultPicker.Items.Clear();

                    while (payslipNoReader.Read())
                    {

                        int dayFrom = Convert.ToInt32(payslipNoReader["DayFrom"]);
                        int dayTo = Convert.ToInt32(payslipNoReader["DayTo"]);
                        string department = payslipNoReader["department"] is DBNull ? string.Empty : payslipNoReader["department"].ToString();

                        // It gives zero to a One digit number example if 1 is value it turns to 01
                        string dayFromZero = dayFrom.ToString().PadLeft(2, '0');
                        string dayToZero = dayTo.ToString().PadLeft(2, '0');

                        // These variables used for the Preview Button to pass it to other page ,so dont deleted it.
                        payslip.dateFrom = dayFromZero;
                        payslip.dateTo = dayToZero;

                        // Getting the value of the require data
                        int empID = payslip.id;
                        string depAbbre = payslip.getDepartmentAbbre(department);
                        string year = yearPicker.SelectedItem.ToString();
                        string monthAbbreviation = payslip.getMonthAbbre(monthPicker.SelectedItem);

                        string result = $"{empID}{depAbbre}-{monthAbbreviation}{dayFromZero}{dayToZero}-{year}";
                        resultPicker.Items.Add(result);
                    }

                    payslipNoReader.Close();
                    sqlconnection.Close();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                // If month and year are not selected, clear the result in the resultPicker
                resultPicker.Items.Clear();
            }
        }

        private async void Preview_Clicked(object sender, EventArgs e)
        {
            // They should have value
            string valMonth = monthPicker.SelectedItem as string;
            string valYear = yearPicker.SelectedItem as string;
            string valPayslipNum = resultPicker.SelectedItem as string;

            // Checks if there a value to year, month and payslip textbox, data Validation
            if (string.IsNullOrWhiteSpace(valMonth))
            {
                DisplayAlert("Validation Error", "Please select month.", "OK");
                return;
            }
            else if (string.IsNullOrWhiteSpace(valYear))
            {
                DisplayAlert("Validation Error", "Please select year.", "OK");
                return;
            }
            else if (string.IsNullOrWhiteSpace(valPayslipNum))
            {
                DisplayAlert("Validation Error", "Please select bi-monthly salary date.", "OK");
                return;
            }

            // These variables used to pass the value to other page
            int passEmpID = payslip.id;
            string passPayslipno = "";
            if (resultPicker.SelectedItem != null)
            {
                passPayslipno = resultPicker.SelectedItem.ToString();
            }
            string passMonth = payslip.getMonthAbbre(monthPicker.SelectedItem);
            string passyear = yearPicker.SelectedItem.ToString();
            string passdayFrom = payslip.dateFrom;
            string passdayTo = payslip.dateTo;
            string passFullName = $"{payslip.surname}, {payslip.firstname} {payslip.mname}";

            await Navigation.PushAsync(new EmpPayslipPage(Convert.ToString(passEmpID), passPayslipno, passMonth, passyear,
                                                                          passdayFrom, passdayTo, passFullName)); // Pass the id and the result to the Yourpayslip Page

        }

        private void PopulateYears()
        {
            // This codes for the picker Year
            int advancedYear = DateTime.Now.Year + 1;
            int previousYears = DateTime.Now.Year - 6;

            // Clear existing year (if any)
            yearPicker.Items.Clear();

            // Add years dynamically from the current year to the past
            for (int year = advancedYear; year >= previousYears; year--)
            {
                yearPicker.Items.Add(year.ToString());
            }
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await userInterface.backAndLogout(this, "Logout", "Are you sure you want to log out?");
        }

        protected override bool OnBackButtonPressed()
        {
            // if True it preventing to go back in the previous page if accidentally clicked the back button
            return true;
        }

        private async void ChangeCredentials_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangeCredentialsPage(Convert.ToString(payslip.id)));
        }
    }

}