using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using ModelLayer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using DataLayer.Constants.DBConnection;
using Microsoft.EntityFrameworkCore;
using DataLayer.Utilities.Token;

namespace DataLayer.Utilities.Emial
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        private readonly IJwtToken _jwtToken;

        public EmailService(IConfiguration config, DataContext dataContext, IJwtToken jwtToken)
        {
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _jwtToken = jwtToken ?? throw new ArgumentNullException(nameof(jwtToken));
        }

        public async Task<bool> SendEmailAsync(EmailDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.To))
                {
                    return false;
                }

                var userEntity = await _dataContext.User.FirstOrDefaultAsync(user =>
                    user.Email.Equals(request.To));

                if (userEntity == null)
                {
                    return false;
                }

                var token = _jwtToken.GenerateJwtToken(userEntity.Id.ToString(), userEntity.Email);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["SmtpService:Username"]));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = "Forgot Password";
                email.Body = new TextPart(TextFormat.Html) { Text = token };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _configuration["SmtpService:Host"],
                    int.Parse(_configuration["SmtpService:Port"]),
                    SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _configuration["SmtpService:Username"],
                    _configuration["SmtpService:Password"]
                );

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception details here
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
