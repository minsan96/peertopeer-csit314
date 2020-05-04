using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrontEnd.Models;
using PeerToPeerDTO;
using Newtonsoft.Json;

namespace FrontEnd.Pages.QPages
{
    public class CreateModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;

        public Users _currentUser;
        public CreateModel(FrontEnd.Models.QnaContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Home Page.  
                return RedirectToPage("/Login");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return RedirectToPage("/Login");
            }

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            return Page();
        }

        [BindProperty]
        public Questions Questions { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!User.Identity.IsAuthenticated)
            {
                // Home Page.  
                return RedirectToPage("/Login");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return RedirectToPage("/Login");
            }
            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add default values 
            Questions.CreatedDate = DateTime.Now;
            Questions.CreatedBy = _currentUser.ID;
            Questions.Rating = 0;

            _context.Questions.Add(Questions);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}