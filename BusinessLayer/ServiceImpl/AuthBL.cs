using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using ModelLayer.Model.DTO;

namespace BusinessLayer.ServiceImpl
{
    public class AuthBL : IAuthService
    {
        private readonly IAuth _authrepo;

        public AuthBL(IAuth authrepo)
        {
            _authrepo = authrepo;
        }
        public Task<ResponseBody<string>> AuthenticateAsync(LoginDto login)
        {
            return _authrepo.AuthenticateAsync(login);
        }

        public Task<ResponseBody<bool>> ForgottPasswordAsync(EmailDto emailDto)
        {
            return _authrepo.ForgottPasswordAsync(emailDto);
        }
    }
}
