using DevTaskTracker.Domain.Entities;

namespace DevTaskTracker.Application.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
