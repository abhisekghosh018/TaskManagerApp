using DevTaskTracker.Application.IServices;
using DevTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevTaskTracker.Infrastructure.Services
{
    public class EmailChecker : IEmailChecker
    {
        private readonly AppDbContext _appDbContext;

        public EmailChecker(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _appDbContext.Members.AnyAsync(m => m.WorkEmail == email);
                //|| await _appDbContext.Users.AnyAsync(u=>u.Email ==email);
        }
    }
}
