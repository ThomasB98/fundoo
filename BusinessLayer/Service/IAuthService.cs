using DataLayer.Constants.ResponeEntity;
using ModelLayer.Model.DTO;

namespace BusinessLayer.Service
{
    public interface IAuthService
    {
        Task<ResponseBody<string>> AuthenticateAsync(LoginDto login);
        Task<ResponseBody<bool>> ForgottPasswordAsync(EmailDto emailDto);
    }
}
