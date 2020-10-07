using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RevatureP1.Models
{
    /// <summary>
    /// Location data plus associated data for user output
    /// </summary>
    public class LoginViewModel
    {
        private string username;
        private string password;


        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string Username { get => username; set => username = value; }

        [StringLength(40, MinimumLength = 2)]
        [Required]
        public string Password { get => password; set => password = value; }
    }
}
