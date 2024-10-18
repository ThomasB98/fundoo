using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.DTO.Label
{
    public class LabelRequestDto
    {
        public string? Name { get; set; }

        public int noteId { get; set; }
    }
}
