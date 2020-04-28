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

        public IActionResult OnPostFindUser()
        {
            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                return new JsonResult("User not found!");
            }
            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            return new JsonResult("Founded user");
        }
    }
}
