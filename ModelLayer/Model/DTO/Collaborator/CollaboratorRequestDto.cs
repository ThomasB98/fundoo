using ModelLayer.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO.Collaborator
{
    public class CollaboratorRequestDto
    {
        /// <summary>
        /// The identifier of the Note associated with this Collaborator.
        /// </summary>
        [Required]
        public int NoteId { get; set; }

        /// <summary>
        /// The identifier of the User who is collaborating.
        /// </summary>
        [Required]
        public int CollaboratorId { get; set; }

        /// <summary>
        /// The name of the collaborating User.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Collaborator name must be between 1 and 100 characters.")]
        public string CollaboratorName { get; set; }

        /// <summary>
        /// The name of the Note being collaborated on.
        /// </summary>
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Note name must be between 1 and 200 characters.")]
        public string NoteName { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
