using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTaskTracker.Domain.Entities
{
    public class Organization
    {
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public ICollection<AppUser> Users { get; set; }
    }
}
