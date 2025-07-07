using DevTaskTracker.Application.IdentityService;
using DevTaskTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Infrastructure.IdentityService
{
    public class IdentityServices : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        public IdentityServices(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DeleteAsync(string id)
        {
          var isUserExits = await _userManager.FindByIdAsync(id);

            if (isUserExits != null) {
               var deleteResult = await _userManager.DeleteAsync(isUserExits);
                if (!deleteResult.Succeeded)
                {
                    // Log the error details
                    var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                    Console.WriteLine($"❌ Failed to delete user {id}: {errors}");
                }
                else
                {
                    Console.WriteLine($"✅ User {id} deleted successfully");
                }
            }
        }
    }
}
