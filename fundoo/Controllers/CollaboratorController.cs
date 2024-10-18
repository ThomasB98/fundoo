using BusinessLayer.Service;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.Enum;
using System.Collections.Generic;

namespace fundoo.Controllers
{
    /// <summary>
    /// API Controller for handling collaboration-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CollaboratorController : Controller
    {
        private readonly ICollaborationService _collaborationService;
        private readonly ICache _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollaboratorController"/> class.
        /// </summary>
        /// <param name="collaborationService">The collaboration service for handling operations.</param>
        /// /// <param name="cacheService">The redis service for handling cacheing.</param>
        public CollaboratorController(ICollaborationService collaborationService, ICache cacheService)
        {
            _collaborationService = collaborationService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Creates a new collaboration for a note.
        /// </summary>
        /// <param name="noteId">The ID of the note to collaborate on.</param>
        /// <param name="collaboratorId">The ID of the user collaborating.</param>
        /// <param name="role">The role of the collaborator.</param>
        /// <returns>A response indicating whether the collaboration was successfully created.</returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCollaborationAsync([FromBody] CollaboratorRequestDto collaboratorRequestDto)
        {
            var result = await _collaborationService.CreateCollaborationAsync(collaboratorRequestDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Retrieves all collaborations for a given note by its ID.
        /// </summary>
        /// <param name="noteId">The ID of the note.</param>
        /// <returns>A list of collaborations for the specified note.</returns>
        [HttpGet("note/{noteId}")]
        [Authorize(Policy = "RequireViewPermission")]
        public async Task<IActionResult> GetCollaborationsByNoteIdAsync(int noteId)
        {
            var cacheData = _cacheService.GetData<IEnumerable<CollaboratorResponceDto>>("collaboration");
            if(cacheData != null && cacheData.Count()>0)
                return Ok(cacheData);
            var result = await _collaborationService.GetCollaborationsByNoteIdAsync(noteId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves all collaborations for a given user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of collaborations for the specified user.</returns>
        [HttpGet("user/{userId}")]
        [Authorize(Policy = "RequireViewPermission")]
        public async Task<IActionResult> GetCollaborationsByUserIdAsync(int userId)
        {
            var cacheData = _cacheService.GetData<IEnumerable<CollaboratorDto>>("collaboration");
            if (cacheData != null && cacheData.Count() > 0)
                return Ok(cacheData);

            var result = await _collaborationService.GetCollaborationsByUserIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);

            
        }

        /// <summary>
        /// Updates the role of a collaborator in a specific collaboration.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration to update.</param>
        /// <param name="role">The new role to assign to the collaborator.</param>
        /// <returns>A response indicating whether the role was successfully updated.</returns>
        [HttpPut("update/{collaborationId}")]
        [Authorize(Policy = "RequireEditPermission")]
        public async Task<IActionResult> UpdateCollaborationAsync(int collaborationId, Role role)
        {
            var result = await _collaborationService.UpdateCollaborationAsync(collaborationId, role);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Deletes a collaboration by its ID.
        /// </summary>
        /// <param name="collaborationId">The ID of the collaboration to delete.</param>
        /// <returns>A response indicating whether the collaboration was successfully deleted.</returns>
        [HttpDelete("delete/{collaborationId}")]
        [Authorize(Policy = "RequireDeletePermission")]
        public async Task<IActionResult> DeleteCollaborationAsync(int collaborationId)
        {
            var result = await _collaborationService.DeleteCollaborationAsync(collaborationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Adds a collaborator to a note.
        /// </summary>
        /// <param name="noteId">The ID of the note to which the collaborator is being added.</param>
        /// <param name="collaboratorId">The ID of the user being added as a collaborator.</param>
        /// <param name="role">The role of the new collaborator.</param>
        /// <returns>A response indicating whether the collaborator was successfully added.</returns>
        [HttpPost("add-collaborator")]
        [Authorize(Policy = "RequireAddrPermission")]
        public async Task<IActionResult> AddCollaboratorAsync(int noteId, int collaboratorId, Role role)
        {
            var result = await _collaborationService.AddCollaboratorAsync(noteId, collaboratorId, role);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Removes a collaborator from a note.
        /// </summary>
        /// <param name="noteId">The ID of the note from which the collaborator is being removed.</param>
        /// <param name="collaboratorId">The ID of the user being removed as a collaborator.</param>
        /// <returns>A response indicating whether the collaborator was successfully removed.</returns>
        [HttpDelete("remove-collaborator")]
        [Authorize(Policy = "RequireRemovePermission")]
        public async Task<IActionResult> RemoveCollaboratorAsync(int noteId, int collaboratorId)
        {
            var result = await _collaborationService.RemoveCollaboratorAsync(noteId, collaboratorId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
