using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO;

namespace fundoo.Controllers
{
    /// <summary>
    /// API Controller for handling user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service to be used for user operations.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="newUser">The details of the user to be created.</param>
        /// <returns>The created user details.</returns>
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserDto newUser)
        {
            var result = await _userService.CreateUserAsync(newUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>Indicates whether the deletion was successful.</returns>
        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user details if found.</returns>
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user details if found.</returns>
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// Updates a user's details.
        /// </summary>
        /// <param name="userDto">The updated user details.</param>
        /// <returns>The updated user details.</returns>
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(userDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// Sets the active status of a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="isActive">The active status to be set.</param>
        /// <returns>Indicates whether the operation was successful.</returns>
        [HttpPatch("set-active-status/{userId}")]
        [Authorize]
        public async Task<IActionResult> SetUserActiveStatusAsync(int userId, bool isActive)
        {
            var result = await _userService.SetUserActiveStatusAsync(userId, isActive);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
