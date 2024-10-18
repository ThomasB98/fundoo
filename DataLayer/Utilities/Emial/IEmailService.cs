using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Emial
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(EmailDto request);
    }
}
