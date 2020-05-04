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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questions = await _context.Questions.FirstOrDefaultAsync(m => m.ID == id);

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

            return RedirectToPage("./Index");
        }

        private bool QuestionsExists(int id)
        {
            return _context.Questions.Any(e => e.ID == id);
        }
    }
}
