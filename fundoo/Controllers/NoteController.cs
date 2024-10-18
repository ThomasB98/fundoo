using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO.Note;

namespace fundoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NoteRequestDto noteRequestDto)
        {
            if(ModelState.IsValid)
            {
                var response = await _noteService.CreateNoteAsync(noteRequestDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] NoteDto noteDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.UpdateNoteAsync(noteDto);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpDelete("{noteId}")]
        public async Task<IActionResult> DeleteNoteAsync(int noteId)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.DeleteNoteAsync(noteId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNoteByIdAsync(int noteId)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.GetNoteByIdAsync(noteId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotesByUserIdAsync(int userId)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.GetNotesByUserIdAsync(userId);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpPut("archive/{noteId}")]
        public async Task<IActionResult> ArchiveNoteAsync(int noteId, [FromQuery] bool isArchived)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.ArchiveNoteAsync(noteId, isArchived);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpPut("pin/{noteId}")]
        public async Task<IActionResult> PinNoteAsync(int noteId, [FromQuery] bool isPinned)
        {
            if (ModelState.IsValid)
            {
                var response = await _noteService.PinNoteAsync(noteId, isPinned);
                if (response.Success)
                {
                    return Ok(response);
                }
                return StatusCode((int)response.StatusCode, response);
            }
            return BadRequest();    
        }
    }
}

