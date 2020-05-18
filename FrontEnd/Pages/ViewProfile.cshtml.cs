using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PeerToPeerDTO;

namespace FrontEnd.Pages
{
    public class ViewProfileModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public ViewProfileModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        [BindProperty]
        public Users editUser { get; set; }
        [BindProperty]
        public List<Questions> Questions { get; set; }
        [BindProperty]
        public List<Answers> Answers { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Home Page.  
                return RedirectToPage("./Index");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return RedirectToPage("./Index");
            }

            if (id == 0)
            {
                return RedirectToPage("./Index");
            }
            
            editUser = await _apiClient.GetUsers(id);
            Questions = await _apiClient.GetQuestionsByUserId(id);
            Answers = await _apiClient.GetAnswersByUserId(id);
            
            // Info.  
            return Page();
        }
    }
}