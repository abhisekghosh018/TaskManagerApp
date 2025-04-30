using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DevTaskTracker.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public ICollection<TaskItem> AssignedTask { get; set; } = new List<TaskItem>();
        public string FullName { get; set; }

        public string PersonName {get; set; } 

        public string? OrganizationId { get; set; }

        public Organization? Organization { get; set; }
    }
}
