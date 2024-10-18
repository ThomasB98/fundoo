using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO.Note;

namespace BusinessLayer.Service
{
    public interface INoteService
    {
        Task<ResponseBody<bool>> ArchiveNoteAsync(int noteId, bool isArchived);
        Task<ResponseBody<NoteResponseDto>> CreateNoteAsync(NoteRequestDto noteRequestDto);
        Task<ResponseBody<bool>> DeleteNoteAsync(int noteId);
        Task<ResponseBody<NoteDto>> GetNoteByIdAsync(int noteId);
        Task<ResponseBody<IEnumerable<NoteDto>>> GetNotesByUserIdAsync(int userId);
        Task<ResponseBody<bool>> PinNoteAsync(int noteId, bool isPinned);
        Task<ResponseBody<NoteDto>> UpdateNoteAsync(NoteDto noteDto);
    }
}
