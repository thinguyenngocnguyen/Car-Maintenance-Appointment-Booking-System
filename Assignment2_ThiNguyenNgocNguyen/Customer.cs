using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2_ThiNguyenNgocNguyen
{
    public class Customer
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string MakeModel { get; set; }
        public int Year { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Problem { get; set; }

        public Customer(string customerName, string address,string city, string province, string postalCode, 
            string homePhone, string cellPhone,string email, 
            string makeModel, int year, DateTime appointmentDate, string problem)
        {
            CustomerName = customerName;
            Address = address;
            City = city;
            Province = province;
            PostalCode = postalCode;
            HomePhone = homePhone;
            CellPhone = cellPhone;
            Email = email;
            MakeModel = makeModel;
            Year = year;
            AppointmentDate = appointmentDate;
            Problem = problem;

        }


    }
}
