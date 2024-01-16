using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2_ThiNguyenNgocNguyen
{
    public partial class BookCarMaintenance : Form
    {
        private List<Customer> customers;
        public BookCarMaintenance()
        {
            InitializeComponent();
            customers = new List<Customer>();
        }

        private void BookCarMaintenance_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnPreFill_Click(object sender, EventArgs e)
        {
            PreFillForm();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Customer customer = SaveAppointment();
                customers.Add(customer);
                SaveToFile(customer);
                MessageBox.Show("Appointment booked successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreFillForm()
        {
            txtName.Text = "Ngoc";
            txtAddress.Text = "5 University Ave";
            txtCity.Text = "Waterloo";
            txtProvince.Text = "ON";
            txtPostalCode.Text = "N2M6K8";
            txtHomePhone.Text = "123-456-7890";
            txtCellPhone.Text = "987-654-3210";
            txtEmail.Text = "ngoc@example.com";
            txtMakeModel.Text = "Honda Odyssey";
            txtYear.Text = "2015";
            dtpAppointmentDate.Value = DateTime.Now;
            txtProblem.Text = "Cooling fan noise";
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errors.AppendLine("Customer name is required.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    errors.AppendLine("Address is required when email is provided.");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(txtCity.Text))
                {
                    errors.AppendLine("City is required when email is provided.");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(txtProvince.Text))
                {
                    errors.AppendLine("Province is required when email is provided.");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(txtPostalCode.Text))
                {
                    errors.AppendLine("Postal code is required when email is provided.");
                    isValid = false;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtCity.Text) ||
                    string.IsNullOrWhiteSpace(txtProvince.Text) || string.IsNullOrWhiteSpace(txtPostalCode.Text))
                {
                    errors.AppendLine("Postal information is required when email is not provided.");
                    isValid = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtProvince.Text) && !ValidationHelper.IsValidProvinceCode(txtProvince.Text))
            {
                errors.AppendLine("Invalid province code. Please enter a valid two-letter code.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtPostalCode.Text) && !ValidationHelper.IsValidPostalCode(txtPostalCode.Text))
            {
                errors.AppendLine("Invalid postal code. Please enter a valid Canadian postal code.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtHomePhone.Text) && string.IsNullOrWhiteSpace(txtCellPhone.Text))
            {
                errors.AppendLine("At least one phone number (home or cell) is required.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                errors.AppendLine("Invalid email address. Please enter a valid email.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtMakeModel.Text))
            {
                errors.AppendLine("Make & model is required.");
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(txtYear.Text))
            {
                if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
                {
                    errors.AppendLine("Invalid year. Please enter a valid year between 1900 and the current year plus one.");
                    isValid = false;
                }
            }

            if (dtpAppointmentDate.Value < DateTime.Today)
            {
                errors.AppendLine("Appointment date cannot be in the past.");
                isValid = false;
            }

            if (!isValid)
            {
                lblMessage.Text = errors.ToString();
                lblMessage.Visible = true;
                if (!string.IsNullOrWhiteSpace(txtName.Text))
                    txtName.Focus();
            }
            else
            {
                lblMessage.Visible = false;
            }

            return isValid;
        }

        private Customer SaveAppointment()
        {
            string name = ValidationHelper.Capitalize(txtName.Text.Trim());
            string address = txtAddress.Text.Trim();
            string province = txtProvince.Text.ToUpper();
            string city = ValidationHelper.Capitalize(txtCity.Text.Trim());
            string postalCode = txtPostalCode.Text.ToUpper();
            string homePhone = FormatPhoneNumber(txtHomePhone.Text.Trim());
            string cellPhone = FormatPhoneNumber(txtCellPhone.Text.Trim());
            string email = txtEmail.Text.ToLower();
            string carMakeModel = ValidationHelper.Capitalize(txtMakeModel.Text.Trim());
            int year = string.IsNullOrWhiteSpace(txtYear.Text) ? 0 : int.Parse(txtYear.Text);
            DateTime appointmentDate = dtpAppointmentDate.Value.Date;
            string problem = txtProblem.Text.Trim();

            return new Customer(name, address, province, city, postalCode, homePhone, cellPhone, email,
                                carMakeModel, year, appointmentDate, problem);
        }

        private void SaveToFile(Customer customer)
        {
            string appointmentLine = $"{FormatString(customer.CustomerName)}\n{FormatString(customer.Address)}\n" +
                $"{FormatString(customer.City)}\n{FormatString(customer.Province)}\n{FormatString(customer.PostalCode)}\n" +
                $"{FormatString(customer.HomePhone)}\n{FormatString(customer.CellPhone)}\n{FormatString(customer.Email)}\n" +
                $"{FormatString(customer.MakeModel)}\n{customer.Year}\n{customer.AppointmentDate.ToString("yyyy-MM-dd")}\n{FormatString(customer.Problem)}";


            try
            {
                using (StreamWriter writer = File.AppendText("appointments.txt"))
                {
                    writer.WriteLine(appointmentLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatString(string input)
        {
            return input?.Replace("\n", "<br>").Replace("|", "<pipe>") ?? "";
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            return Regex.Replace(phoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ResetForm()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtProvince.Text = "";
            txtPostalCode.Text = "";
            txtHomePhone.Text = "";
            txtCellPhone.Text = "";
            txtEmail.Text = "";
            txtMakeModel.Text = "";
            txtYear.Text = "";
            dtpAppointmentDate.Value = DateTime.Now;
            txtProblem.Text = "";

            lblMessage.Text = "";
            lblMessage.Visible = false;

            txtName.Focus();
        }
    }
}




