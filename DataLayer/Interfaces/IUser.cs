using DataLayer.Constants.Class;
using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    /// <summary>
    /// Interface for managing user-related operations in the system.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Asynchronously creates a new user in the system.
        /// </summary>
        /// <param name="user">The user data transfer object (DTO) containing the user information.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing the created user's information.</returns>
        Task<ResponseBody<UserDto>> CreateUserAsync(UserDto user);

        /// <summary>
        /// Asynchronously updates an existing user's information in the system.
        /// </summary>
        /// <param name="user">The user data transfer object (DTO) containing the updated user information.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing the updated user's information.</returns>
        Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto user);

        /// <summary>
        /// Asynchronously deletes a user from the system based on the provided user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to delete.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object indicating whether the user was successfully deleted.</returns>
        Task<ResponseBody<bool>> DeleteUserAsync(int userId);

        /// <summary>
        /// Asynchronously retrieves a user's information by their unique ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to retrieve.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing the requested user's information.</returns>
        Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId);

        /// <summary>
        /// Asynchronously retrieves a user's information by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object containing the requested user's information.</returns>
        Task<ResponseBody<UserDto>> GetUserByEmailAsync(string email);

        /// <summary>
        /// Asynchronously sets the active status of a user in the system.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <param name="isActive">A boolean indicating whether the user should be active or inactive.</param>
        /// <returns>A <see cref="ResponseBody{T}"/> object indicating whether the operation was successful.</returns>
        Task<ResponseBody<bool>> SetUserActiveStatusAsync(int id, bool isActive);
    }
}

