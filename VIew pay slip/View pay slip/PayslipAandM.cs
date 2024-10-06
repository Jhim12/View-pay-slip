using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace View_pay_slip
{
    internal class PayslipAandM
    {
        public int id { get; set; }
        public int empid { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string mname { get; set; }
        public string designation { get; set; }
        public string status { get; set; }
        public string department { get; set; }
        public decimal basicpay { get; set; }
        public decimal allowance { get; set; }
        public decimal overload { get; set; }
        public decimal longevity { get; set; }
        public decimal other1 { get; set; }
        public decimal other2 { get; set; }
        public decimal grosspay { get; set; }
        public decimal wtax { get; set; }
        public decimal sss { get; set; }
        public decimal med { get; set; }
        public decimal peraa { get; set; }
        public decimal pagibig { get; set; }
        public decimal sssloan { get; set; }
        public decimal peraaloan { get; set; }
        public decimal ca { get; set; }
        public decimal tardiness { get; set; }
        public decimal absent { get; set; }
        public string otherdeduction1 { get; set; }
        public string otherdeduction2 { get; set; }
        public decimal deduction1 { get; set; }
        public decimal deduction2 { get; set; }
        public decimal pagibigloan { get; set; }
        public decimal pagibigloanc { get; set; }
        public decimal others { get; set; }
        public decimal totaldeduc { get; set; }
        public decimal net { get; set; }

        // I created this attributes
        public string fullname { get; set; }
        public string currentDate { get; set; }
        public string bimonthlydate { get; set; }
        public string payslipno { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string CurrentUsername { get; set; }
        public string NewUsername { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string getMonthAbbre(object selectedItem)
        {
            if (selectedItem == null || string.IsNullOrEmpty(selectedItem.ToString()))
            {
                return "Null!";
            }
            else
            {
                string monthAbbre = selectedItem.ToString();
                switch (monthAbbre)
                {
                    case "January":
                        return "JAN";
                    case "February":
                        return "FEB";
                    case "March":
                        return "MAR";
                    case "April":
                        return "APR";
                    case "May":
                        return "MAY";
                    case "June":
                        return "JUN";
                    case "July":
                        return "JUL";
                    case "August":
                        return "AUG";
                    case "September":
                        return "SEP";
                    case "October":
                        return "OCT";
                    case "November":
                        return "NOV";
                    case "December":
                        return "DEC";
                    default:
                        return "Null!";
                }
            }
        }

        public string getDepartmentAbbre(string department)
        {
            // Implement switch-case to get month abbreviation
            string convDept = department;
            switch (convDept)
            {
                case "Administration": //
                    return "ADM";
                case "Elementary":
                    return "GS";
                case "Highschool":
                    return "HS";
                case "College":
                    return "COL";
                case "ST John":
                    return "SJ";
                case "Junior HS":
                    return "JHS";
                case "Senior HS":
                    return "SHS";
                case "Others":
                    return "O";
                case "SRC Annex1":
                    return "SJ";
                default:
                    return "Null!";
            }
        }

        public string getMonthNumber(object selectedItem)
        {
            if (selectedItem == null || string.IsNullOrEmpty(selectedItem.ToString()))
            {
                return "Null!";
            }
            else
            {
                string monthnum = selectedItem.ToString();
                switch (monthnum)
                {
                    case "January":
                        return "1";
                    case "February":
                        return "2";
                    case "March":
                        return "3";
                    case "April":
                        return "4";
                    case "May":
                        return "5";
                    case "June":
                        return "6";
                    case "July":
                        return "7";
                    case "August":
                        return "8";
                    case "September":
                        return "9";
                    case "October":
                        return "10";
                    case "November":
                        return "11";
                    case "December":
                        return "12";
                    default:
                        return "Null!";
                }
            }
        }


    }
}