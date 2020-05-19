using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;
using PeerToPeerDTO;
using Newtonsoft.Json;

namespace FrontEnd.Pages.QPages
{
    public class DeleteModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;

        public DeleteModel(FrontEnd.Models.QnaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Questions Questions { get; set; }

        public Users _currentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return RedirectToPage("./Index");
            }

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);

            if (_currentUser.UserType != "Moderator")
            {
                return RedirectToPage("./Index");
            }

            Questions = await _context.Questions.FirstOrDefaultAsync(m => m.ID == id);

            if (Questions == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questions = await _context.Questions.FindAsync(id);

            try
            {
                if (Questions != null)
                {
                    _context.Questions.Remove(Questions);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
