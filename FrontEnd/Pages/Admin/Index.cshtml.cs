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
    public class IndexModel : PageModel
    {
        protected readonly IApiClient _apiClient;

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public List<Users> _userList { get; set; }

        [BindProperty]
        public List<Users> _mostratedusers { get; set; }

        [BindProperty]
        public Users _currentUser { get; set; }

        public async Task OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Login");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                Response.Redirect("/Login");
            }
            else
            {
                _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
                if (_currentUser.UserType != "Admin")
                {
                    Response.Redirect("/Index");
                }
            }
            _userList = await _apiClient.GetUsers();
            _mostratedusers = await _apiClient.GetTopRatedUsers();
        }
    }
}