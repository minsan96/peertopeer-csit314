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
    public class DeleteCommentModel : PageModel
    {
        protected readonly IApiClient _apiClient;
        public DeleteCommentModel(IApiClient apiClient)
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

            if (_currentUser.UserType != "Moderator")
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
            if (Comments.ID == 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt.");
                return Page();
            }

            try
            {
                await _apiClient.DeleteComments(Comments.ID);
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
            return RedirectToPage("./Index");
        }
    }
}