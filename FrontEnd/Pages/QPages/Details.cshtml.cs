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
using FrontEnd.Services;

namespace FrontEnd.Pages.QPages
{
    public class DetailsModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;
        protected readonly IApiClient _apiClient;
        public DetailsModel(FrontEnd.Models.QnaContext context, IApiClient apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        public Questions Questions { get; set; }
        public IList<Answers> Answers { get; set; }

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
            else
            {
                var _answers = await _apiClient.GetAnswers();
                Answers = _answers;
            }
            return Page();
        }
    }
}
