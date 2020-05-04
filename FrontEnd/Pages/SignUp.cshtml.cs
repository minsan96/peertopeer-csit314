using FrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeerToPeerDTO;

namespace FrontEnd.Pages
{
    [AllowAnonymous]
    [Authorize]
    public class SignUpModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public SignUpModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Users User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var firstname = User.FirstName;
            var lastname = User.LastName;
            var username = User.UserName;
            var password = User.Password;
            User.UserType = "Normal";
            User.ID = 0;
            User.Rating = 0;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            if (User == null || string.IsNullOrEmpty(User.UserName) || string.IsNullOrEmpty(User.FirstName) || string.IsNullOrEmpty(User.LastName) || string.IsNullOrEmpty(User.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            var login = await _apiClient.CreateUsers(User);
            ViewData["Message"] = "Sign Up Success";
            return Page();
        }
    }
}