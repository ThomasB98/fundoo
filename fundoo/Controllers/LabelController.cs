using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO.Label;


namespace fundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : Controller
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] LableCreationRequestDto labelCreationRequestDto)
        {
            var result = await _labelService.CreateLabelAsync(labelCreationRequestDto);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // GET api/label
        [HttpGet]
        public async Task<IActionResult> GetAllLabels()
        {
            var result = await _labelService.GetAllLabelsAsync();

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // GET api/label/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLabelById(int id)
        {
            var result = await _labelService.GetLabelByIdAsync(id);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // PUT api/label
        [HttpPut]
        public async Task<IActionResult> UpdateLabel([FromBody] LableUpdateRequestDto labelUpdateRequestDto)
        {
            var result = await _labelService.UpdateLabelAsync(labelUpdateRequestDto);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // DELETE api/label/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            var result = await _labelService.DeleteLabelAsync(id);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // POST api/label/addtonote
        [HttpPost("addtonote")]
        public async Task<IActionResult> AddLabelToNote([FromQuery] int noteId, [FromQuery] int labelId)
        {
            var result = await _labelService.AddLabelToNoteAsync(noteId, labelId);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }

        // GET api/label/bynote/{noteId}
        [HttpGet("bynote/{noteId}")]
        public async Task<IActionResult> GetLabelsByNoteId(int noteId)
        {
            var result = await _labelService.GetLabelsByNoteIdAsync(noteId);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode((int)result.StatusCode, result);
        }
    }
}
