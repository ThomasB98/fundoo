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
    /// Interface for managing user authentication and token handling.
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// Authenticates the user and generates an authentication token (e.g., JWT).
        /// </summary>
        /// <param name="login">Login DTO with credentials.</param>
        /// <returns>A token if authentication is successful.</returns>
        Task<ResponseBody<string>> AuthenticateAsync(LoginDto login);

        Task<ResponseBody<bool>> ForgottPasswordAsync(EmailDto emailDto);
    }
}
