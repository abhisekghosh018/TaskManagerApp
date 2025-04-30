using DevTaskTracker.Domain.Entities;

namespace DevTaskTracker.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
