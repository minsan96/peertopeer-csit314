using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PeerToPeerDTO;

namespace FrontEnd.Pages.Admin
{
    public class CreateUserModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public CreateUserModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Users _currentUser { get; set; }

        public IActionResult OnGetAuthenticateUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Home Page.  
                return new JsonResult("Return to Login!");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return new JsonResult("Return to Login!");
            }

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            if (_currentUser.UserType != "Admin")
            {
                return new JsonResult("Return to Index!");
            }
            // Info.  
            return new JsonResult("Valid user");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var firstname = _currentUser.FirstName;
            var lastname = _currentUser.LastName;
            var username = _currentUser.UserName;
            var password = _currentUser.Password;
            _currentUser.ID = 0;
            _currentUser.Rating = 0;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            if (User == null || string.IsNullOrEmpty(_currentUser.UserName) || string.IsNullOrEmpty(_currentUser.FirstName) || string.IsNullOrEmpty(_currentUser.LastName) || string.IsNullOrEmpty(_currentUser.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            try
            {
                var login = await _apiClient.CreateUsers(_currentUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("500"))
                {
                    ModelState.AddModelError(string.Empty, "Internal Server Error.");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Attempt.");
                    return Page();
                }
            }
            ViewData["Message"] = "Create User Success";
            return Page();
        }
    }
}