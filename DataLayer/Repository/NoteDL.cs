using DataLayer.Constants.DBConnection;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net;
using DataLayer.Constants.Exceptions;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Entity;
using AutoMapper;
using ModelLayer.Model.DTO.Note;

namespace DataLayer.Repository
{
    public class NoteDL : INote
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public NoteDL(DataContext context, IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ResponseBody<bool>> ArchiveNoteAsync(int noteId, bool isArchived)
        {
            try
            {
                // Get the logged-in user's ID from the claims
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new UnauthorizedAccessException("User is not logged in.");
                }

                int loggedInUserId = int.Parse(userIdClaim.Value);

                // Validate the note ID
                if (noteId <= 0)
                {
                    throw new ArgumentException("Invalid note ID provided.");
                }

                // Fetch the note from the database
                var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == noteId && n.UserId == loggedInUserId);
                if (note == null)
                {
                    throw new NoteNotFoundException($"No note found with ID: {noteId} for the logged-in user.");
                }

                // Update the archive status
                note.IsArchived = isArchived;

                // Save changes
                var changes = await _context.SaveChangesAsync();
                if (changes > 0)
                {                   
                    return new ResponseBody<bool>
                    {
                        Data = true,
                        Success = true,
                        Message = $"Note {(isArchived ? "archived" : "unarchived")} successfully.",
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    throw new Exception("Error occurred while archiving the note.");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ResponseBody<bool> {
                    Data = false,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.Unauthorized
                };
            }
            catch (NoteNotFoundException ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
                
            }
        }


        public async Task<ResponseBody<NoteResponseDto>> CreateNoteAsync(NoteRequestDto noteRequestDto)
        {
            try
            {
                // Get the logged-in user's ID from the claims
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new UnauthorizedAccessException("User is not logged in.");
                }

                int loggedInUserId = int.Parse(userIdClaim.Value);

                if(loggedInUserId !=noteRequestDto.UserId)
                {
                    throw new UnauthorizedAccessException("User is not logged in.");
                }
                else
                {
                    var noteEntity = _mapper.Map<Note>(noteRequestDto);
                    noteEntity.CreatedDate = DateTime.UtcNow;
                    noteEntity.ModifiedDate = DateTime.UtcNow;

                    await _context.Note.AddAsync(noteEntity);
                    int changes = await _context.SaveChangesAsync();

                    var note=await _context.Note.FirstOrDefaultAsync(c=>c.Id==noteEntity.Id);

                    if (changes > 0)
                    {
                        var noteEntityDto=_mapper.Map<NoteResponseDto>(note);
                        return new ResponseBody<NoteResponseDto>
                        {
                            Data = noteEntityDto,
                            Success = true,
                            Message = "Note saved",
                            StatusCode = HttpStatusCode.OK
                        };
                     
                    }
                    return new ResponseBody<NoteResponseDto>
                    {
                        Data = null,
                        Success = false,
                        Message = "Note saved",
                        StatusCode = HttpStatusCode.InternalServerError
                    };

                }
                

            }
            catch(UserNotFoundException ex)
            {
                return new ResponseBody<NoteResponseDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<NoteResponseDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ResponseBody<bool>> DeleteNoteAsync(int noteId)
        {
            if(noteId == null){
                throw new ArgumentNullException(nameof(noteId));
            }
            try
            {
                var noteEntity = await _context.Note.FirstOrDefaultAsync(note=>note.Id.Equals(noteId));
                if (noteEntity == null)
                {
                    throw new InvalidNoteIdException("Invalid not id");
                }
                _context.Note.Remove(noteEntity);

                int changes= await _context.SaveChangesAsync();
                if (changes <= 0)
                {
                    throw new DataBaseException("Internal DataBase Error");
                }
                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    Message = "Note saved",
                    StatusCode = HttpStatusCode.Accepted
                };
            }
            catch(ArgumentNullException ex)
            {
                return new ResponseBody<bool>
                {
                    Data=false,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ResponseBody<NoteDto>> GetNoteByIdAsync(int noteId)
        {
            try
            {
                if (noteId <=0)
                {
                    throw new ArgumentNullException(nameof(noteId));
                }
                var noteEntiy = await _context.Note.FirstOrDefaultAsync(note => note.Id == noteId);

                if (noteEntiy == null)
                {
                    throw new NoteNotFoundException($"Note with {noteId} not found");
                }
                var noteEntityDto = _mapper.Map<NoteDto>(noteEntiy);
                return new ResponseBody<NoteDto>
                {

                    Data = noteEntityDto,
                    Success = true,
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK

                };

            }
            catch (ArgumentNullException ex)
            {
                return new ResponseBody<NoteDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message + " noteId cannot be null or Zero",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (NoteNotFoundException ex)
            {
                return new ResponseBody<NoteDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message + " invalid id",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ResponseBody<IEnumerable<NoteDto>>> GetNotesByUserIdAsync(int userId)
        {
            try
            {
                if(userId <= 0)
                {
                    throw new Exception("UserId cannot be zero/negative");
                }
                var notesList = await _context.Note
                                      .Where(note => note.UserId == userId)  // Assuming Note has a UserId field
                                      .ToListAsync();
                if (notesList == null || !notesList.Any())
                {
                    throw new UserNotFoundException("No notes found for the specified user");
                }
                var noteDtos = _mapper.Map<IEnumerable<NoteDto>>(notesList);
                return new ResponseBody<IEnumerable<NoteDto>>
                {
                    Data = noteDtos,
                    Success = true,
                    Message = "",
                    StatusCode = HttpStatusCode.OK
                };
            }catch(UserNotFoundException ex)
            {
                return new ResponseBody<IEnumerable<NoteDto>>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch(Exception ex)
            {
                return new ResponseBody<IEnumerable<NoteDto>>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ResponseBody<bool>> PinNoteAsync(int noteId, bool isPinned)
        {
            var response = new ResponseBody<bool>();

            try
            {
                var note = await _context.Note.FindAsync(noteId);

                if (note == null)
                {
                    throw new NoteNotFoundException("Note not found");
                }

                note.IsPinned = isPinned;

                _context.Note.Update(note);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = isPinned ? "Note pinned successfully" : "Note unpinned successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (NoteNotFoundException ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }


        public async Task<ResponseBody<NoteDto>> UpdateNoteAsync(NoteDto noteDto)
        {
            try
            {
                if (noteDto == null)
                {
                    throw new ArgumentNullException("noteDto cannot be null");
                }

                var existingNote = await _context.Note.FindAsync(noteDto);
                if (existingNote == null)
                {
                    throw new NoteNotFoundException($"No note found with ID {noteDto.Id}");
                }

                existingNote.Title = noteDto.Title;
                existingNote.Content = noteDto.Content;
                existingNote.IsPinned = noteDto.IsPinned;

                _context.Note.Update(existingNote);
                await _context.SaveChangesAsync();

                var updatedNoteDto = _mapper.Map<NoteDto>(existingNote);
                return new ResponseBody<NoteDto>
                {
                    Data = updatedNoteDto,
                    Success = true,
                    Message = "Note updated successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (ArgumentNullException ex)
            {
                return new ResponseBody<NoteDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (NoteNotFoundException ex)
            {
                return new ResponseBody<NoteDto>
                {
                    Data = null,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<NoteDto>
                {
                    Data = null,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

    }
}
