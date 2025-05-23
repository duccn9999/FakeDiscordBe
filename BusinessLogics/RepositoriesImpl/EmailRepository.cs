using BusinessLogics.Repositories;
using DataAccesses.DTOs.Emails;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.RepositoriesImpl
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;

        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(SendEmailDTO email)
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(email.From);
                message.To.Add(new MailAddress(email.To));
                message.Subject = email.Subject;
                message.Body = email.Body;
                message.IsBodyHtml = true; // Set to true if sending HTML email

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = _configuration["EmailSettings:SmtpHost"];
                    smtpClient.Port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                    smtpClient.Credentials = new System.Net.NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    smtpClient.EnableSsl = true; // Set to true if using SSL

                    await smtpClient.SendMailAsync(message);
                }
            }
        }
    }
}
