using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.DTO.Label;
namespace DataLayer.Interfaces
{
    public interface ILabel
    {
        Task<ResponseBody<LabelCreationResponseDto>> CreateLabelAsync(LableCreationRequestDto lableCreationRequestDto);
        Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetAllLabelsAsync();
        Task<ResponseBody<LabelResponseDto>> GetLabelByIdAsync(int labelId);
        Task<ResponseBody<bool>> UpdateLabelAsync(LableUpdateRequestDto label);
        Task<ResponseBody<bool>> DeleteLabelAsync(int labelId);

        Task<ResponseBody<bool>> AddLabelToNoteAsync(int noteId, int labelId);
        Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetLabelsByNoteIdAsync(int noteId);
    }
}
