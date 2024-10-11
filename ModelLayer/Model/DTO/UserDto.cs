using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO
{
    /// <summary>
    /// Represents a Data Transfer Object for the User entity.
    /// Used to transfer user data between layers of the application.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the user's full name.
        /// The name is required and must be between 2 and 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// The email is required and must be a valid email format.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password for the user.
        /// The password is required but should be stored as a hash.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets whether the user account is active.
        /// Default is true (active).
        /// </summary>
        public bool IsActive { get; set; } = false;
    }
}
