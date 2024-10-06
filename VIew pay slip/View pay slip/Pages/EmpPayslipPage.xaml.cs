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
    public partial class EmpPayslipPage : ContentPage
    {
        UserInterface userInterface = new UserInterface();
        public EmpPayslipPage(string passEmpID, string passPayslipno, string passMonth, string passYear, string passDayFrom, string passDayTo, string passFullName) // Got these from prev page
        {
            InitializeComponent();

            // sql connection
            Dbconnection dbconnection = new Dbconnection();
            SqlConnection sqlconnection = new SqlConnection(dbconnection.sqlConnection());

            userInterface.removePageControls(this);

            try
            {
                sqlconnection.Open();
                List<PayslipAandM> empInfoList = new List<PayslipAandM>();

                // First SQL query execution
                string queryString = $"Select * from dbo.payroll WHERE empid = @id AND payslipno = @payslipno";
                SqlCommand command = new SqlCommand(queryString, sqlconnection);
                command.Parameters.AddWithValue("@id", passEmpID); // Parameter
                command.Parameters.AddWithValue("@payslipno", passPayslipno); // Parameter
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    empInfoList.Add(new PayslipAandM
                    {
                        id = Convert.ToInt32(reader["empid"]),
                        fullname = passFullName,
                        designation = $"{reader["designation"]}",
                        status = $"{reader["status"]}",
                        department = $"{reader["department"]}"

                    });
                }
                reader.Close(); // Close the first reader

                sqlconnection.Close();

                string FormatBimonthlydate = passDayFrom + "-" + passMonth + "-" + passYear + " " + "|" + " " + passDayTo + "-" + passMonth + "-" + passYear;
                empInfoList.ForEach(item => item.bimonthlydate = FormatBimonthlydate);

                EmpInfo.ItemsSource = empInfoList;
            }
            catch (Exception ex)
            {
                throw;
            }
            //

            try
            {
                sqlconnection.Open();
                List<PayslipAandM> empPayslip = new List<PayslipAandM>();
                string queryString = $"Select * from dbo.payroll WHERE empid= @empid AND payslipno = @payslipno";
                SqlCommand command = new SqlCommand(queryString, sqlconnection);
                command.Parameters.AddWithValue("@empid", passEmpID); // Parameter
                command.Parameters.AddWithValue("@payslipno", passPayslipno); // Parameter
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    {
                        // Grosspay
                        decimal basicpay = reader["basicpay"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["basicpay"]);
                        decimal allowance = reader["allowance"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["allowance"]);
                        decimal overload = reader["overload"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["overload"]);
                        decimal longevity = reader["longevity"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["longevity"]);
                        decimal other1 = reader["other1"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["other1"]);
                        decimal other2 = reader["other2"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["other2"]);

                        // Function of grosspay
                        decimal grosspay = basicpay + allowance + overload + longevity + other1 + other2;

                        // Function Deduction
                        decimal wtax = reader["wtax"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["wtax"]);
                        decimal sss = reader["sss"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["sss"]);
                        decimal med = reader["med"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["med"]);
                        decimal peraa = reader["peraa"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["peraa"]);
                        decimal pagibig = reader["pagibig"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["pagibig"]);
                        decimal sssloan = reader["sssloan"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["sssloan"]);
                        decimal peraaloan = reader["peraaloan"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["peraaloan"]);
                        decimal ca = reader["ca"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["ca"]);
                        decimal tardiness = reader["tardiness"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["tardiness"]);
                        decimal absent = reader["absent"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["absent"]);
                        decimal deduction1 = reader["deduction1"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["deduction1"]);
                        decimal deduction2 = reader["deduction2"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["deduction2"]);
                        decimal pagibigloan = reader["pagibigloan"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["pagibigloan"]);
                        decimal pagibigloanc = reader["pagibigloanc"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["pagibigloanc"]);
                        decimal others = reader["others"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["others"]);

                        // Function for total deduction
                        decimal totaldeduc = wtax + sss + med + peraa + pagibig + sssloan + peraaloan + pagibigloan + pagibigloanc + others + ca + tardiness + absent + deduction1 + deduction2;

                        empPayslip.Add(new PayslipAandM
                        {
                            //
                            payslipno = passPayslipno,
                            //
                            basicpay = basicpay,
                            allowance = allowance,
                            overload = overload,
                            longevity = longevity,
                            other1 = other1,
                            other2 = other2,
                            grosspay = grosspay,
                            //
                            wtax = wtax,
                            sss = sss,
                            med = med,
                            peraa = peraa,
                            pagibig = pagibig,
                            sssloan = sssloan,
                            peraaloan = peraaloan,
                            ca = ca,
                            tardiness = tardiness,
                            absent = absent,
                            deduction1 = deduction1,
                            deduction2 = deduction2,
                            pagibigloan = pagibigloan,
                            pagibigloanc = pagibigloanc,
                            others = others,
                            totaldeduc = totaldeduc,
                            // Other deduction these are string variables, above are decimals
                            otherdeduction1 = reader["otherdeduction1"] == DBNull.Value ? string.Empty : reader["otherdeduction1"].ToString(),
                            otherdeduction2 = reader["otherdeduction2"] == DBNull.Value ? string.Empty : reader["otherdeduction2"].ToString(),
                            //
                            net = reader["net"] == DBNull.Value ? 0.0m : Convert.ToDecimal(reader["net"])

                        });
                    }


                }
                reader.Close();
                sqlconnection.Close();

                MyNetpay.ItemsSource = empPayslip;
                MyTax.ItemsSource = empPayslip;
                MyDeduc.ItemsSource = empPayslip;
            }
            catch (Exception ex)
            {
                throw;
            }



        }

        protected override bool OnBackButtonPressed()
        {
            userInterface.backAndLogout(this, "Back", "Are you sure you want to back in the previous page?");
            return true;
        }


    }
}