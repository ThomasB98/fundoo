using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Interfaces;

namespace BusinessLayer.ServiceImpl
{
    public class UserBL : IUserService
    {
        private readonly IUser _userRepo;

        public UserBL(IUser userRepo)
        {
            _userRepo = userRepo;
        }
        public Task<ResponseBody<UserDto>> CreateUserAsync(UserDto newUser)
        {
            return _userRepo.CreateUserAsync(newUser);
        }

        public Task<ResponseBody<bool>> DeleteUserAsync(int userId)
        {
            return _userRepo.DeleteUserAsync(userId);
        }

        public Task<ResponseBody<UserDto>> GetUserByEmailAsync(string email)
        {
            return _userRepo.GetUserByEmailAsync(email);
        }

        public Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId)
        {
            return _userRepo.GetUserByIdAsync(userId);
        }

        public Task<ResponseBody<bool>> SetUserActiveStatusAsync(int userId, bool isActive)
        {
            return _userRepo.SetUserActiveStatusAsync(userId, isActive);
        }

        public Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto userDto)
        {
            return _userRepo.UpdateUserAsync(userDto);
        }
    }
}
