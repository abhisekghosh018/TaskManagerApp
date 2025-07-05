using DevTaskTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.IServices
{

    public interface IUserIdentityService
    {
        Task<(AppUser user, string? UserId, IEnumerable<IdentityError> Errors)> CreateUserAsync(AppUser user, string password, string role);
    }
}
