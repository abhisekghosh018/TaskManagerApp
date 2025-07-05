using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Application.Interfaces
{
    public interface IOrganization
    {
        Task<bool> IsOrganizationExits(string name);
        
    }

}
