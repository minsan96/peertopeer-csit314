using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PeerToPeerDTO;

namespace FrontEnd.Pages.QPages
{
    public class EditCommentModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        public EditCommentModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Comments Comments { get; set; }

        public Users _currentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToPage("./Index");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return RedirectToPage("./Index");
            }

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            Comments = await _apiClient.GetComments(id);

            if (_currentUser.UserType != "Moderator" && Comments.CreatedBy != _currentUser.ID)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Comments.ID == 0 || string.IsNullOrEmpty(Comments.Description))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt.");
                return Page();
            }

            try
            {
                await _apiClient.PutComments(Comments.ID, Comments);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    ModelState.AddModelError(string.Empty, "This comment does not exist.");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error has occurred.");
                    return Page();
                }
            }
            ViewData["Message"] = "Edit Success";
            return Page();
        }
    }
}