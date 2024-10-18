using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.Entity;
using ModelLayer.Model.Enum;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Interface representing collaboration-related operations for the application.
    /// </summary>
    public interface ICollaborationService
    {
        /// <summary>
        /// Creates a new collaboration for a note.
        /// </summary>
        /// <param name="noteId">The ID of the note to collaborate on.</param>
        /// <param name="collaboratorId">The ID of the user collaborating.</param>
        /// <param name="role">The role of the collaborator.</param>
        /// <returns>A <see cref="ResponseBody{CollaborationDto}"/> containing the created collaboration details.</returns>
        Task<ResponseBody<CollaboratorResponceDto>> CreateCollaborationAsync(CollaboratorRequestDto collaboratorRequestDto);

        /// <summary>
        /// Retrieves all collaborations for a given note by its ID.
        /// </summary>
        /// <param name="noteId">The ID of the note.</param>
        /// <returns>A <see cref="ResponseBody{List{CollaborationDto}}"/> containing the collaborations for the specified note.</returns>
        Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByNoteIdAsync(int noteId);

        /// <summary>
        /// Retrieves all collaborations for a given user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="ResponseBody{List{CollaborationDto}}"/> containing the collaborations for the specified user.</returns>
        Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByUserIdAsync(int userId);

        /// <summary>
        /// Updates the role of a collaborator in a specific collaboration.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration to update.</param>
        /// <param name="role">The new role to assign to the collaborator.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the role update was successful.</returns>
        Task<ResponseBody<bool>> UpdateCollaborationAsync(int collaborationId, Role role);

        /// <summary>
        /// Deletes a collaboration by its ID.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration to delete.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the deletion was successful.</returns>
        Task<ResponseBody<bool>> DeleteCollaborationAsync(int collaborationId);

        /// <summary>
        /// Adds a collaborator to a note.
        /// </summary>
        /// <param name="noteId">The ID of the note to which the collaborator is being added.</param>
        /// <param name="collaboratorId">The ID of the user being added as a collaborator.</param>
        /// <param name="role">The role of the new collaborator.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the collaborator was successfully added.</returns>
        Task<ResponseBody<bool>> AddCollaboratorAsync(int noteId, int collaboratorId, Role role);

        /// <summary>
        /// Removes a collaborator from a note.
        /// </summary>
        /// <param name="noteId">The ID of the note from which the collaborator is being removed.</param>
        /// <param name="collaboratorId">The ID of the user being removed as a collaborator.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the collaborator was successfully removed.</returns>
        Task<ResponseBody<bool>> RemoveCollaboratorAsync(int noteId, int collaboratorId);

        Task<Collaborator> GetCollaboratorByNoteAndUserAsync(int noteId, int userId);
    }
}
