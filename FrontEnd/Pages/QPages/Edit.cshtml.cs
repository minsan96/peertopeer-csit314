using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;
using PeerToPeerDTO;
using Newtonsoft.Json;

namespace FrontEnd.Pages.QPages
{
    public class EditModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;

        public EditModel(FrontEnd.Models.QnaContext context)
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

            Questions = await _context.Questions.FirstOrDefaultAsync(m => m.ID == id);
            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);

            if (_currentUser.UserType != "Moderator" && Questions.CreatedBy != _currentUser.ID)
            {
                return RedirectToPage("./Index");
            }

            if (Questions == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (string.IsNullOrEmpty(Questions.Question) || string.IsNullOrEmpty(Questions.Description))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }

            _context.Attach(Questions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(Questions.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["Message"] = "Edit Success";
            return Page();
        }

        private bool QuestionsExists(int id)
        {
            return _context.Questions.Any(e => e.ID == id);
        }
    }
}
