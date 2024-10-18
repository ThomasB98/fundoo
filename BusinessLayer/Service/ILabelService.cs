using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO.Label;

namespace BusinessLayer.Service
{
    public interface ILabelService
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
