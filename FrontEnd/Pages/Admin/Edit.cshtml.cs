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
    public class EditModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public EditModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Users _currentUser { get; set; }

        [BindProperty]
        public Users editUser { get; set; }

        public async Task<IActionResult> OnGet(int id = 0)
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

            if (_currentUser.UserType != "Admin")
            {
                return RedirectToPage("./Index");
            }
            // Info.  
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (User == null || string.IsNullOrEmpty(editUser.UserName) || string.IsNullOrEmpty(editUser.FirstName) || string.IsNullOrEmpty(editUser.LastName) || string.IsNullOrEmpty(editUser.Password))
                {
                    ModelState.AddModelError(string.Empty, "Invalid Attempt");
                    return Page();
                }
                await _apiClient.PutUsers(editUser.ID, editUser);
                ViewData["Message"] = "Edit User Success";
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