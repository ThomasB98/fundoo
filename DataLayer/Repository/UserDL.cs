using AutoMapper;
using DataLayer.Constants.DBConnection;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.DTO;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DataLayer.Constants.Exceptions;
using Microsoft.Data.SqlClient.Server;
namespace DataLayer.Repository
{
    public class UserDL : IUser
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserDL(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseBody<UserDto>> CreateUserAsync(UserDto newUser)
        {
            var response=new ResponseBody<UserDto>();
            var oldUser = await _context.User.FirstOrDefaultAsync(uc => uc.Email.Equals(newUser.Email));
            if(oldUser != null)
            {
                response.Data = newUser;
                response.Success = false;
                response.Message = "User Registration Unsucessfull";
                response.StatusCode = HttpStatusCode.Ambiguous;

                return response;
            }
            var userEntity = _mapper.Map<User>(newUser);
            userEntity.CreatedDate = DateTime.Now;
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userEntity.PasswordHash);
            await _context.User.AddAsync(userEntity);
            int changes=await _context.SaveChangesAsync();
            if(changes > 0)
            {
                var createdUserDto = _mapper.Map<UserDto>(userEntity);
                response.Data = createdUserDto;
                response.Success = true;
                response.Message = "User Registration sucessfull";
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
            response.Data = newUser;
            response.Success = false;
            response.Message = "User Registration Unsucessfull erroe occure at database layer";
            response.StatusCode = HttpStatusCode.InternalServerError;

            return response;
        }

        public async Task<ResponseBody<bool>> DeleteUserAsync(int userId)
        {
            var response = new ResponseBody<bool>();
            if (userId <= 0)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "User Id in correct";
                response.StatusCode = HttpStatusCode.OK;
            }
            var user = await _context.User.FirstOrDefaultAsync(uc => uc.Id.Equals(userId));
            if (user == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "User Id in correct";
                response.StatusCode = HttpStatusCode.OK;
            }
            _context.User.Remove(user);
            int chages = await _context.SaveChangesAsync();
            if (chages > 0)
            {
                response.Data = true;
                response.Success = true;
                response.Message = "Account  deleted";
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }

            response.Data = false;
            response.Success = false;
            response.Message = "Account not deleted";
            response.StatusCode = HttpStatusCode.InternalServerError;

            return response;
        }

        public async Task<ResponseBody<UserDto>> GetUserByEmailAsync(string email)
        {
            var response = new ResponseBody<UserDto>();

            try
            {
                // Validate the email input
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new EmptyFieldException(nameof(email), typeof(string));
                }

                // Fetch the user by email
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    throw new UserNotFoundException($"User not found with email address: {email}");
                }

                // Map the user entity to DTO
                var userEntityDto = _mapper.Map<UserDto>(user);

                response.Data = userEntityDto;
                response.Success = true;
                response.Message = "User retrieved successfully.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (EmptyFieldException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An internal error occurred: " + ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response; // Ensure the response is returned in all scenarios
        }

        public async Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId)
        {
            var response = new ResponseBody<UserDto>();

            try
            {
                // Validate the userId input
                if (userId <= 0)
                {
                    throw new InvalidIdException("Invalid user ID. User ID must be greater than zero.");
                }

                // Fetch the user by ID
                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new UserNotFoundException($"User not found with ID: {userId}");
                }

                // Map the user entity to DTO
                var userEntityDto = _mapper.Map<UserDto>(user);

                response.Data = userEntityDto;
                response.Success = true;
                response.Message = "User retrieved successfully.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (InvalidIdException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            catch (UserNotFoundException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An internal error occurred: " + ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response; // Ensure the response is returned in all scenarios
        }

        public async Task<ResponseBody<bool>> SetUserActiveStatusAsync(int id, bool isActive)
        {
            var response = new ResponseBody<bool>();
            try
            {
                if (id <= 0)
                {
                    throw new InvalidIdException("Invalid user ID. User ID must be greater than zero.");
                }

                // Fetch the user by ID
                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new UserNotFoundException($"User not found with ID: {id}");
                }

                user.IsActive = isActive;
                int changes=await _context.SaveChangesAsync();

                if(changes > 0)
                {
                    response.Data = true;
                    response.Success = true;
                    response.Message = "User Active status changed";
                    response.StatusCode = HttpStatusCode.OK;

                    return response;
                }

                throw new Exception("Internal database error");


            }catch(InvalidIdException ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;

                return response;
            }
            catch(UserNotFoundException ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.NotFound;

                return response;
            }
            catch(Exception ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;

                return response;
            }


            return response;

        }

        public async Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto userDto)
        {
            var response = new ResponseBody<UserDto>();
            try
            {
                if(userDto == null)
                {
                    throw new ArgumentNullException();
                }

                var user=await _context.User.FirstOrDefaultAsync(u=>u.Email == userDto.Email);

                if(user == null)
                {
                    throw new UserNotFoundException($"User not found with email {userDto.Email}");
                }

                user=_mapper.Map<User>(userDto);

                int changes=await _context.SaveChangesAsync();

                if (changes > 0)
                {
                    var userEntityDto = _mapper.Map<UserDto>(user);
                    response.Data = userEntityDto;
                    response.Success = true;
                    response.Message = "User Active status changed";
                    response.StatusCode = HttpStatusCode.OK;

                    return response;
                }

                throw new Exception("Internal database error");

            }
            catch(ArgumentNullException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            catch(UserNotFoundException ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.NotFound;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;

                return response;
            }

            return response;
        }
    }
}
