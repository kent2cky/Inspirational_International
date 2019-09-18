using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Inspiration_International.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Inspiration_International.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmYourEmailModel : PageModel
    {
        public IActionResult OnGetAsync(string userId, string code)
        {
            return Page();
        }
    }
}
