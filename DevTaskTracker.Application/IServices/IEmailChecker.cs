using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.IServices
{
    public interface IEmailChecker
    {
        Task<bool> IsEmailExistsAsync(string email);
    }
}
