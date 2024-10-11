using AutoMapper;
using DataLayer.Constants.DBConnection;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Constants.Token;
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

namespace DataLayer.Repository
{
    public class AuthDL : IAuth
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtToken _jwtToken;

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

    }
}
