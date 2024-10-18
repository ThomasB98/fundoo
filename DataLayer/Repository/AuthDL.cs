using AutoMapper;
using DataLayer.Constants.DBConnection;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;
using DataLayer.Constants.Exceptions;
using DataLayer.Utilities.Token;
using DataLayer.Utilities.Emial;

namespace DataLayer.Repository
{
    public class AuthDL : IAuth
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtToken _jwtToken;
        private readonly IEmailService _emailService;

        public AuthDL(DataContext dataContext,IMapper mapper,IJwtToken jwtToken)
        {
            _context = dataContext;
            _mapper = mapper;
            _jwtToken = jwtToken;
        }
        public async Task<ResponseBody<string>> AuthenticateAsync(LoginDto login)
        {
            try
            {
                if (login == null)
                {
                    throw new ArgumentNullException("email password required");
                }
                var userEntity=await _context.User.FirstOrDefaultAsync(user=>user.Email == login.Email);
                if (userEntity == null)
                {
                    throw new UserNotFoundException(login.Email);
                }
                if (BCrypt.Net.BCrypt.Verify(login.Password, userEntity.PasswordHash))
                {
                    var token = _jwtToken.GenerateJwtToken(userEntity.Id.ToString(), userEntity.Email);
                    return new ResponseBody<string>()
                    {
                        Data = token,
                        Success = true,
                        Message = "Login succefull",
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new ResponseBody<string>()
                {
                    Data = "",
                    Success = false,
                    Message = "Login Unsuccefull Invalid Password",
                    StatusCode = HttpStatusCode.BadRequest
                };

            }
            catch(ArgumentNullException ex)
            {
                return new ResponseBody<string>
                {
                    Data=null,
                    Success=false,
                    Message=ex.Message,
                    StatusCode=HttpStatusCode.BadRequest
                };
            }
            catch(UserNotFoundException ex)
            {
                return new ResponseBody<string>
                {
                    Data = null,
                    Success = false,
                    Message ="Incorrect Email"+ ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                // Generic exception handler
                return new ResponseBody<string>
                {
                    Data = null,
                    Success = false,
                    Message = "An error occurred: " + ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ResponseBody<bool>> ForgottPasswordAsync(EmailDto emailDto)
        {
            var response = new ResponseBody<bool>();

            try
            {
                // Check if the emailDto is null or if the email property is null or empty
                if (emailDto == null || string.IsNullOrWhiteSpace(emailDto.To))
                {
                    throw new ArgumentNullException("Email cannot be null or empty.");
                }

                // Call the email service to send the email
                var flag = await _emailService.SendEmailAsync(emailDto);

                if (!flag)
                {
                    return new ResponseBody<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "Invalid email or failed to send email.",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    Message = "Check your email for the password reset instructions.",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (ArgumentNullException ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                // Catch all for other exceptions to return a general error message
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

    }
}
