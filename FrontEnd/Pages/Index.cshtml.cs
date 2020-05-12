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
    public class IndexModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        public Users _currentUser;

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public List<Questions> _mostratedqn1 { get; set; }

        [BindProperty]
        public List<Questions> _mostratedqn2 { get; set; }

        [BindProperty]
        public List<Answers> _mostratedans { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

            _mostratedqn1 = await _apiClient.GetTopRatedQuestions();
            _mostratedqn2 = await _apiClient.GetTopRatedQuestions(5, 30);
            _mostratedans = await _apiClient.GetTopRatedAnswers();

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            // Info.  
            return Page();
        }

        public IActionResult OnPostFindUser()
        {
            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return new JsonResult("User not found!");
            }
            return new JsonResult("Founded user");
        }
    }
}
