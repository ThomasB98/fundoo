using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface INote
    {
        /// <summary>
        /// Creates a new note asynchronously.
        /// </summary>
        /// <param name="noteRequestDto">The DTO containing note data.</param>
        /// <returns>A response containing the created note.</returns>
        Task<ResponseBody<NoteResponseDto>> CreateNoteAsync(NoteRequestDto noteRequestDto);

        /// <summary>
        /// Retrieves a note by its ID asynchronously.
        /// </summary>
        /// <param name="noteId">The ID of the note to retrieve.</param>
        /// <returns>A response containing the note if found.</returns>
        Task<ResponseBody<NoteDto>> GetNoteByIdAsync(int noteId);

        /// <summary>
        /// Retrieves all notes associated with a user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user whose notes to retrieve.</param>
        /// <returns>A response containing a list of notes.</returns>
        Task<ResponseBody<IEnumerable<NoteDto>>> GetNotesByUserIdAsync(int userId);

        /// <summary>
        /// Updates an existing note asynchronously.
        /// </summary>
        /// <param name="noteDto">The DTO containing updated note data.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        Task<ResponseBody<NoteDto>> UpdateNoteAsync(NoteDto noteDto);

        /// <summary>
        /// Deletes a note by its ID asynchronously.
        /// </summary>
        /// <param name="noteId">The ID of the note to delete.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        Task<ResponseBody<bool>> DeleteNoteAsync(int noteId);

        /// <summary>
        /// Pins a note to the top of the user's note list.
        /// </summary>
        /// <param name="noteId">The ID of the note to pin.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        Task<ResponseBody<bool>> PinNoteAsync(int noteId, bool isPinned);

        /// <summary>
        /// Archives a note, marking it as archived.
        /// </summary>
        /// <param name="noteId">The ID of the note to archive.</param>
        /// <returns>A response indicating success or failure of the operation.</returns>
        Task<ResponseBody<bool>> ArchiveNoteAsync(int noteId, bool isArchived);
    }
}
