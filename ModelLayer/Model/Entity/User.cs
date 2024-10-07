using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Entity
{
    /// <summary>
    /// Represents a user in the system.
    /// This class contains all user-related information and relationships.
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password for the user.
        /// This should never store the actual password.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the date when the user account was created.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        // <summary>
        /// Gets or sets whether the user account is active.
        /// Inactive accounts cannot access the system.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the collection of notes associated with this user.
        /// This is a navigation property for the one-to-many relationship between User and Note.
        /// </summary>
        [InverseProperty("User")]
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
