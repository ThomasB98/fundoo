using AutoMapper;
using DataLayer.Constants.Class;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<Login, LoginDto>();
            CreateMap<LoginDto, Login>();
        }
    }
}
