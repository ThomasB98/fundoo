using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using ModelLayer.Model.DTO.Label;

namespace BusinessLayer.ServiceImpl
{
    public class lableBL : ILabelService
    {
        public readonly ILabel _lableRepo;

        public lableBL(ILabel lableRepo)
        {
            _lableRepo = lableRepo;
        }
        public Task<ResponseBody<bool>> AddLabelToNoteAsync(int noteId, int labelId)
        {
            return _lableRepo.AddLabelToNoteAsync(noteId, labelId);
        }

        public Task<ResponseBody<LabelCreationResponseDto>> CreateLabelAsync(LableCreationRequestDto lableCreationRequestDto)
        {
            return _lableRepo.CreateLabelAsync(lableCreationRequestDto);
        }

        public Task<ResponseBody<bool>> DeleteLabelAsync(int labelId)
        {
            return _lableRepo.DeleteLabelAsync(labelId);
        }

        public Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetAllLabelsAsync()
        {
            return _lableRepo.GetAllLabelsAsync();
        }

        public Task<ResponseBody<LabelResponseDto>> GetLabelByIdAsync(int labelId)
        {
            return _lableRepo.GetLabelByIdAsync(labelId);
        }

        public Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetLabelsByNoteIdAsync(int noteId)
        {
            return _lableRepo.GetLabelsByNoteIdAsync((int)noteId);
        }

        public Task<ResponseBody<bool>> UpdateLabelAsync(LableUpdateRequestDto label)
        {
            return _lableRepo.UpdateLabelAsync(label);
        }
    }
}
