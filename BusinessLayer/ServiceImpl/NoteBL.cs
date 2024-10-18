using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using ModelLayer.Model.DTO.Note;

namespace BusinessLayer.ServiceImpl
{
    public class NoteBL : INoteService
    {
        private readonly INote _noteRepo;

        public NoteBL(INote noteRepo)
        {
            _noteRepo = noteRepo;
        }
        public Task<ResponseBody<bool>> ArchiveNoteAsync(int noteId, bool isArchived)
        {
            return _noteRepo.ArchiveNoteAsync(noteId, isArchived);
        }

        public Task<ResponseBody<NoteResponseDto>> CreateNoteAsync(NoteRequestDto noteRequestDto)
        {
            return _noteRepo.CreateNoteAsync(noteRequestDto);
        }

        public Task<ResponseBody<bool>> DeleteNoteAsync(int noteId)
        {
            return _noteRepo.DeleteNoteAsync(noteId);
        }

        public Task<ResponseBody<NoteDto>> GetNoteByIdAsync(int noteId)
        {
            return _noteRepo.GetNoteByIdAsync(noteId);
        }

        public Task<ResponseBody<IEnumerable<NoteDto>>> GetNotesByUserIdAsync(int userId)
        {
            return _noteRepo.GetNotesByUserIdAsync((int)userId);
        }

        public Task<ResponseBody<bool>> PinNoteAsync(int noteId, bool isPinned)
        {
            return _noteRepo.PinNoteAsync(noteId, isPinned);
        }

        public Task<ResponseBody<NoteDto>> UpdateNoteAsync(NoteDto noteDto)
        {
            return _noteRepo.UpdateNoteAsync(noteDto);
        }
    }
}
