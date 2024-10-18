using AutoMapper;
using DataLayer.Constants.DBConnection;
using DataLayer.Constants.Exceptions;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Entity;
using ModelLayer.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Diagnostics;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.DTO;

namespace DataLayer.Repository
{
    public class CollaborationDL : ICollaboration
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICache _cacheService;
        public CollaborationDL(DataContext context,IMapper mapper, IHttpContextAccessor httpContextAccessor, ICache cacheService)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cacheService = cacheService;
        }

        public async Task<ResponseBody<CollaboratorResponceDto>> CreateCollaborationAsync(CollaboratorRequestDto collaboratorRequestDto)
        {
            try
            {
                // Retrieve the note
                var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == collaboratorRequestDto.NoteId);
                if (note == null)
                {
                    throw new NoteNotFoundException("Note not found");
                }

                // Retrieve the collaborator user
                var collaboratorUser = await _context.User.FirstOrDefaultAsync(c => c.Id == collaboratorRequestDto.CollaboratorId);
                if (collaboratorUser == null)
                {
                    throw new UserNotFoundException("Collaborator not found");
                }

                // Check if collaboration already exists for the requested collaborator
                var existingCollaborator = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.NoteId == collaboratorRequestDto.NoteId && c.CollaboratorId == collaboratorRequestDto.CollaboratorId);
                if (existingCollaborator != null)
                {
                    throw new Exception("Collaboration already exists for this user.");
                }

                //get loggedin user id
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new UnauthorizedAccessException("User is not logged in.");
                }

                int loggedInUserId = int.Parse(userIdClaim.Value);

                // Check if collaboration exists for the note's owner
                var ownerCollaborator = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.NoteId == collaboratorRequestDto.NoteId && c.CollaboratorId == note.UserId);

                // If owner collaboration does not exist, create it with role OWNER
                if (ownerCollaborator == null)
                {
                    var ownerEntity = new Collaborator
                    {
                        NoteId = note.Id,
                        CollaboratorId = note.UserId,
                        Role = Role.OWNER, 
                        Created_at = DateTime.UtcNow,
                        Updated_at = DateTime.UtcNow
                    };

                    var ownerEntityDto=_mapper.Map<CollaboratorDto>(ownerEntity);

                    var expiryTime = DateTimeOffset.Now.AddMinutes(3);
                    _cacheService.SetData<CollaboratorDto>(
                        $"collaboration{ownerEntity.Id}",
                        ownerEntityDto,
                        expiryTime
                        );
                    await _context.Collaborator.AddAsync(ownerEntity);
                    await _context.SaveChangesAsync();                    
                }

                // Create the new collaboration for the requested user
                var collaboratorEntity = _mapper.Map<Collaborator>(collaboratorRequestDto);
                collaboratorEntity.Note = note;
                collaboratorEntity.CollaboratorUser = collaboratorUser;
                collaboratorEntity.Created_at = DateTime.UtcNow; // Set Created_at to current UTC time
                collaboratorEntity.Updated_at = DateTime.UtcNow; // Set Updated_at to current UTC time

                
                _context.Collaborator.Add(collaboratorEntity);

                // Save changes
                await _context.SaveChangesAsync();

                // Map to response DTO
                var resultDto = _mapper.Map<CollaboratorResponceDto>(collaboratorEntity);

                var expiryTime1 = DateTimeOffset.Now.AddMinutes(3);
                _cacheService.SetData<CollaboratorResponceDto>(
                        $"collaboration{resultDto.Id}",
                        resultDto,
                        expiryTime1
                        );


                return new ResponseBody<CollaboratorResponceDto>
                {
                    Data = resultDto,
                    Success = true,
                    StatusCode = HttpStatusCode.Created,
                    Message = "Collaboration created successfully."
                };
            }
            catch (NoteNotFoundException ex)
            {
                return new ResponseBody<CollaboratorResponceDto>
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<CollaboratorResponceDto>
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> DeleteCollaborationAsync(int Id)
        {
            var LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            try
            {
                var existingCollaborator = await _context.Collaborator.FirstOrDefaultAsync(c=>c.Id==Id);
                if (existingCollaborator == null)
                {
                    throw new CollaborationNotExistsException("No collaboration exists");
                }

                _context.Collaborator.Remove(existingCollaborator);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Collaboration deleted successfully."
                };


            }
            catch(Exception ex)
            {
                return new ResponseBody<bool>()
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }

        }

        public async Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByNoteIdAsync(int noteId)
        {
            try
            {
                var ListOfCollaboration = await _context.Collaborator
                    .Where(c => c.NoteId == noteId)
                    .ToListAsync();

                if (ListOfCollaboration.Count == 0)
                {
                    return new ResponseBody<List<CollaboratorDto>>
                    {
                        Data = null,
                        Success = true,
                        StatusCode = HttpStatusCode.NoContent,
                        Message = "No Collaboration found"
                    };
                }

                var ListOfCollaborationDto = ListOfCollaboration
                    .Select(c => _mapper.Map<CollaboratorDto>(c))
                    .ToList();

                var expiryTime = DateTimeOffset.Now.AddMinutes(3);
                _cacheService.SetData<IEnumerable<CollaboratorDto>>(
                    "collaboration",
                    ListOfCollaborationDto,
                    expiryTime
                    );
                return new ResponseBody<List<CollaboratorDto>>
                {
                    Data = ListOfCollaborationDto,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<List<CollaboratorDto>>()
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }

        }

        public async Task<ResponseBody<List<CollaboratorDto>>> GetCollaborationsByUserIdAsync(int userId)
        {
            try
            {
                var ListOfCollaboration = await _context.Collaborator
                    .Where(c => c.Note != null && c.Note.UserId == userId)
                    .ToListAsync();

                if (ListOfCollaboration.Count == 0)
                {
                    return new ResponseBody<List<CollaboratorDto>>
                    {
                        Data = null,
                        Success = false,
                        StatusCode = HttpStatusCode.NoContent,
                        Message = "No data Found"
                    };
                }

                var ListOfCollaborationDto = ListOfCollaboration
                    .Select(c => _mapper.Map<CollaboratorDto>(c))
                    .ToList();
                var expiryTime = DateTimeOffset.Now.AddMinutes(3);
                _cacheService.SetData<IEnumerable<CollaboratorDto>>(
                    "collaboration",
                    ListOfCollaborationDto,
                    expiryTime
                    );
                return new ResponseBody<List<CollaboratorDto>>
                {
                    Data = ListOfCollaborationDto,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<List<CollaboratorDto>>()
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> RemoveCollaboratorAsync(int noteId, int collaboratorId)
        {
            try
            {
                var existingCollaborator = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == collaboratorId);

                if (existingCollaborator == null)
                {
                    throw new CollaborationNotExistsException("Collaborator not found for this note.");
                }

                _context.Collaborator.Remove(existingCollaborator);
                await _context.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Collaborator removed successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> UpdateCollaborationAsync(int collaborationId, Role role)
        {
            try
            {
                var existingCollaborator = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.Id == collaborationId);

                if (existingCollaborator == null)
                {
                    throw new CollaborationNotExistsException("Collaboration not found.");
                }

                existingCollaborator.Role = role;
                existingCollaborator.Updated_at = DateTime.UtcNow; // Update the timestamp

                _context.Collaborator.Update(existingCollaborator);
                await _context.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Collaboration updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> AddCollaboratorAsync(int noteId, int userId, Role role)
        {
            try
            {
                // Check if the note exists
                var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == noteId);
                if (note == null)
                {
                    throw new NoteNotFoundException("Note not found.");
                }

                // Check if the user exists
                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found.");
                }

                // Check if the user is already a collaborator on this note
                var existingCollaboration = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == userId);

                if (existingCollaboration != null)
                {
                    // If the user already exists as a collaborator, update the role
                    existingCollaboration.Role = role;
                    existingCollaboration.Updated_at = DateTime.UtcNow; // Update the timestamp
                    _context.Collaborator.Update(existingCollaboration);
                }
                else
                {
                    // Otherwise, create a new collaboration
                    var newCollaboration = new Collaborator
                    {
                        NoteId = noteId,
                        CollaboratorId = userId,
                        Role = role,
                        Created_at = DateTime.UtcNow,  // Set Created_at to current UTC time
                        Updated_at = DateTime.UtcNow   // Set Updated_at to current UTC time
                    };
                    _context.Collaborator.Add(newCollaboration);
                }

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Collaborator added/updated successfully."
                };
            }
            catch (NoteNotFoundException ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ex.Message
                };
            }
            catch (UserNotFoundException ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                // General exception handling
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while adding/updating the collaborator: " + ex.Message
                };
            }
        }

        public async Task<Collaborator> GetCollaboratorByNoteAndUserAsync(int noteId, int userId)
        {
            try
            {
                // Query the Collaborator entity to find the matching record
                var collaborator = await _context.Collaborator
                    .FirstOrDefaultAsync(c => c.NoteId == noteId && c.CollaboratorId == userId);

                if (collaborator == null)
                {
                    throw new CollaborationNotExistsException("No collaborator found for this note and user.");
                }

                return collaborator;
            }
            catch (CollaborationNotExistsException ex)
            {
                // Handle specific case where the collaboration doesn't exist
                Debug.WriteLine("Error: " + ex.Message);  // Logging
                return null;
            }
            catch (Exception ex)
            {
                // General exception handling
                Debug.WriteLine("Error: " + ex.Message);  // Logging
                return null;
            }
        }
    }
}
