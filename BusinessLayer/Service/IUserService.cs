using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Interface representing user-related operations for the application.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="newUser">The details of the user to be created.</param>
        /// <returns>A <see cref="ResponseBody{UserDto}"/> containing the created user details.</returns>
        Task<ResponseBody<UserDto>> CreateUserAsync(UserDto newUser);

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the deletion was successful.</returns>
        Task<ResponseBody<bool>> DeleteUserAsync(int userId);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A <see cref="ResponseBody{UserDto}"/> containing the user details if found.</returns>
        Task<ResponseBody<UserDto>> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="ResponseBody{UserDto}"/> containing the user details if found.</returns>
        Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId);

        /// <summary>
        /// Sets the active status of a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="isActive">The new active status of the user.</param>
        /// <returns>A <see cref="ResponseBody{bool}"/> indicating whether the operation was successful.</returns>
        Task<ResponseBody<bool>> SetUserActiveStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Updates the details of an existing user.
        /// </summary>
        /// <param name="userDto">The updated user details.</param>
        /// <returns>A <see cref="ResponseBody{UserDto}"/> containing the updated user details.</returns>
        Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto userDto);
    }
}
