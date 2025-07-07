using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.IdentityService
{
    public interface IIdentityService
    {
        Task DeleteAsync(string id);
    }
}
