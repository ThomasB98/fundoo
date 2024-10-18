using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.Entity;
using ModelLayer.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Interface for managing collaboration-related operations in the system.
    /// </summary>
    public interface ICollaboration
    {
        /// <summary>
        /// Asynchronously creates a new collaboration between a note and a user.
        /// </summary>
        /// <param name="noteId">The ID of the note to collaborate on.</param>
        /// <param name="collaboratorId">The ID of the user who will collaborate on the note.</param>
        /// <param name="role">The role assigned to the collaborator.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object indicating whether the collaboration was successfully created.</returns>
        Task<ResponseBody<CollaboratorResponceDto>> CreateCollaborationAsync(CollaboratorRequestDto collaboratorRequestDto);

        /// <summary>
        /// Asynchronously retrieves all collaborations for a given note.
        /// </summary>
        /// <param name="noteId">The ID of the note.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing a list of collaborations for the specified note.</returns>
        Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByNoteIdAsync(int noteId);

        /// <summary>
        /// Asynchronously retrieves all collaborations for a given user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing a list of collaborations for the specified user.</returns>
        Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByUserIdAsync(int userId);

        /// <summary>
        /// Asynchronously updates the role of a collaborator in a collaboration.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration.</param>
        /// <param name="role">The new role to assign to the collaborator.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object indicating whether the role was successfully updated.</returns>
        Task<ResponseBody<bool>> UpdateCollaborationAsync(int collaborationId, Role role);

        /// <summary>
        /// Asynchronously deletes a collaboration from the system.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration to delete.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object indicating whether the collaboration was successfully deleted.</returns>
        Task<ResponseBody<bool>> DeleteCollaborationAsync(int collaborationId);

        /// <summary>
        /// Adds a collaborator to a specified note.
        /// </summary>
        /// <param name="noteId">The ID of the note to which the collaborator is being added.</param>
        /// <param name="collaboratorId">The ID of the user being added as a collaborator.</param>
        /// <param name="role">The role assigned to the collaborator.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the collaborator was successfully added.</returns>
        Task<ResponseBody<bool>> RemoveCollaboratorAsync(int noteId, int collaboratorId);

        /// <summary>
        /// Removes a collaborator from a specified note.
        /// </summary>
        /// <param name="noteId">The ID of the note from which the collaborator is being removed.</param>
        /// <param name="collaboratorId">The ID of the user being removed as a collaborator.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the collaborator was successfully removed.</returns>
        Task<ResponseBody<bool>> AddCollaboratorAsync(int noteId, int collaboratorId, Role role);

        Task<Collaborator> GetCollaboratorByNoteAndUserAsync(int noteId, int userId);
    }
}
