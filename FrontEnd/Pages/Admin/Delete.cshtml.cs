using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeerToPeerDTO;

namespace FrontEnd.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public DeleteModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }


        [BindProperty]
        public Users Users { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int userid = id ?? default(int);

            Users = await _apiClient.GetUsers(userid);

            if (Users == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Users.ID == 0)
            {
                return NotFound();
            }

            try
            {
                await _apiClient.DeleteUsers(Users.ID);
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    ModelState.AddModelError(string.Empty, "This user does not exist.");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Attempt.");
                    return Page();
                }
            }
            return RedirectToPage("./Index");
        }
    }
}