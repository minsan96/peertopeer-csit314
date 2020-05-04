using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;
using PeerToPeerDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FrontEnd.Pages.QPages
{
    public class IndexModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;
        protected readonly IApiClient _apiClient;
        public Users _currentUser;

        public IndexModel(FrontEnd.Models.QnaContext context, IApiClient apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }

        [BindProperty]
        public IList<Questions> Questions { get;set; }

        public Users SingleUser { get; set; }

        public List<string> CreatorNames = new List<string>();

        public async Task OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Home Page.  
                Response.Redirect("/Login");
            }

            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                Response.Redirect("/Login");
            }

            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            Questions = await _context.Questions
                .AsNoTracking()
                .ToListAsync();
            foreach (Questions q in Questions){
                int userid = q.CreatedBy;
                var tempUser = await _apiClient.GetUsers(userid);
                SingleUser = tempUser;
                string name = SingleUser.FirstName + " " + SingleUser.LastName;
                CreatorNames.Add(name);
            }
        }

    }
}
