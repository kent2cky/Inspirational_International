using System;
using Microsoft.AspNetCore.Identity;

namespace Inspiration_International.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}