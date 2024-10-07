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
    /// Represents a note in the system.
    /// This class contains all note-related information and its relationship to a user.
    /// </summary>
    [Table("Notes")]
    public class Note
    {
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the color of the note for display purposes.
        /// </summary>
        [StringLength(50)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets whether the note is pinned for quick access.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets whether the note is archived.
        /// </summary>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets whether the note is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date when the note was created.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date when the note was last modified.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who owns this note.
        /// This is a foreign key to the Users table.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property for the user who owns this note.
        /// This represents the many-to-one relationship between Note and User.
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
