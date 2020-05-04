using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;
using PeerToPeerDTO;

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questions = await _context.Questions.FindAsync(id);

            if (Questions != null)
            {
                _context.Questions.Remove(Questions);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
