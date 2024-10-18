using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.Enum;

namespace ModelLayer.Model.Entity
{
    [Table("Collaborator")]
    public class Collaborator
    {
        /// <summary>
        /// The unique identifier for the Collaborator.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The identifier of the Note associated with this Collaborator.
        /// </summary>
        [ForeignKey("Note")]
        public int NoteId { get; set; }

        /// <summary>
        /// Navigation property for the associated Note.
        /// </summary>
        public Note? Note { get; set; }

        /// <summary>
        /// The identifier of the User who is collaborating.
        /// </summary>
        [ForeignKey("CollaboratorUser")]
        public int CollaboratorId { get; set; }

        /// <summary>
        /// Navigation property for the collaborating User.
        /// </summary>
        public User CollaboratorUser { get; set; }

        /// <summary>
        /// The date and time when the entity was created.
        /// This field is required and must be a valid date.
        /// </summary>
        [DataType(DataType.Date)]
        [Required]
        public DateTime Created_at { get; set; }

        /// <summary>
        /// The date and time when the entity was last updated.
        /// This field is required and must be a valid date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime Updated_at { get; set; }

        // Define the role for this specific collaboration
        public Role Role { get; set; }

    }
}
