using System;
using Microsoft.AspNetCore.Identity;

namespace Inspiration_International.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
    }
}