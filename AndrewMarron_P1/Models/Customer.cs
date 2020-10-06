using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string UserName { get => userName; set => userName = value; }

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string FirstName { get => firstName; set => firstName = value; }

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string LastName { get => lastName; set => lastName = value; }

        [StringLength(40, MinimumLength = 2)]
        [Required]
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
