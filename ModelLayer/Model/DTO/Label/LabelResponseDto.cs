using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO.Label
{
    public class LabelResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NoteId { get; set; }
    }
}
