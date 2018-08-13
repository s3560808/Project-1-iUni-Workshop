using System;

namespace iUni_Workshop.Models.EmployeeModels
{
    public class Employee
    {
        public Employee(String userId, String name, String phoneNumber, String ContactEmail, String photo, String schooName, String livingSuburb)
        {
            UserId = userId;
            Name = name;
            PhoneNumber = phoneNumber;
            Photo = photo;
            SchooName = schooName;
            LivingSuburb = livingSuburb;
        }

        public Employee()
        {
        }

        public String UserId;
        public String Name;
        public String PhoneNumber;
        public String Photo;
        public String SchooName;
        public String LivingSuburb;
    }
}