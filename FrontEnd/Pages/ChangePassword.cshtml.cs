using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PeerToPeerDTO;

namespace FrontEnd.Pages
{
    public class ChangePasswordModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        PasswordHasher<Users> hasher = new PasswordHasher<Users>(
            new OptionsWrapper<PasswordHasherOptions>(
                new PasswordHasherOptions()
                {
                    CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                }
            )
        );

        public ChangePasswordModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Users _currentUser { get; set; }
        [BindProperty]
        public Users editUser { get; set; }
        [BindProperty]
        public string oldpassword { get; set; }
        [BindProperty]
        public string newpassword1 { get; set; }
        [BindProperty]
        public string newpassword2 { get; set; }
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

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            editUser = await _apiClient.GetUsers(id);

            // Info.  
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (editUser.ID == 0)
                {
                    ModelState.AddModelError(string.Empty, "Invalid User. Please try again.");
                    return Page();
                }
                if (hasher.VerifyHashedPassword(editUser, editUser.Password, oldpassword) == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "Incorrect old Password.");
                    return Page();
                }
                if (newpassword1 != newpassword2)
                {
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return Page();
                }
                editUser.Password = newpassword1;
                await _apiClient.ChangePassword(editUser.ID, editUser.Password);
                ViewData["Message"] = "Change Password Success";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                {
                    ModelState.AddModelError(string.Empty, "Invalid Attempt.");
                    return Page();
                }
                else if (ex.Message.Contains("404"))
                {
                    ModelState.AddModelError(string.Empty, "This user does not exist.");
                    return Page();
                }
                else if (ex.Message.Contains("500"))
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
            return Page();
        }
    }
}