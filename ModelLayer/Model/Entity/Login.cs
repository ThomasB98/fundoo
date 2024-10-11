using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Class
{
    /// <summary>
    /// Represents the login credentials of a user.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>The user's email address.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <value>The user's password.</value>
        public string Password { get; set; }
    }
}
