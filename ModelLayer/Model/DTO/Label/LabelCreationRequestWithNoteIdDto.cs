using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO.Label
{
    public class LabelCreationRequestWithNoteIdDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Label name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [Required]
        public int NoteId { get; set; }
    }
}
