using System;
using System.Collections.Generic;
using System.Text;

namespace RevatureP1.Models
{
    public class Customer
    {
        /// <summary>
        /// This stores information about each customer's account
        /// This object corresponds with the database table Cusomers
        /// </summary>
        private int customerId;
        private string userName;
        private string firstName;
        private string lastName;
        private string password;
        private DateTime dateAdded;
        public int CustomerId { get => customerId; set => customerId = value; }
        public string UserName { get => userName; set => userName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Password { get => password; set => password = value; }
        public DateTime DateAdded { get => dateAdded; set => dateAdded = value; }
        public Customer() { }
        public Customer(string UserNameIn, string FirstNameIn, string LastNameIn, string PasswordIn)
        {
            UserName = UserNameIn;
            FirstName = FirstNameIn;
            LastName = LastNameIn;
            Password = PasswordIn;
            DateAdded = DateTime.Now;
        }
    }
}
