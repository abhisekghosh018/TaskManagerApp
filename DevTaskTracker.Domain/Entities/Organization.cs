using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Domain.Entities
{
    public class Organization
    {
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? WebSite { get; set; }

        public ICollection<AppUser> Users { get; set; }
    }
}
