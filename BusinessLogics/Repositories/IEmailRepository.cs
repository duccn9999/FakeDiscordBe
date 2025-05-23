using DataAccesses.DTOs.Emails;

namespace BusinessLogics.Repositories
{
    public interface IEmailRepository
    {
        public Task SendEmail(SendEmailDTO model);
    }
}
