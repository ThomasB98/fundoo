using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IAuthService
    {
        Task<ResponseBody<string>> AuthenticateAsync(LoginDto login);
    }
}
