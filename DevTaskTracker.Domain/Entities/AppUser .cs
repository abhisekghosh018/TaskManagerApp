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
        //public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
        public virtual ICollection<Member> Members { get; set; } = new List<Member>();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string OrganizationId { get; set; }
        public string? Role { get; set; }
        public Organization? Organization { get; set; }
        
    }
}
