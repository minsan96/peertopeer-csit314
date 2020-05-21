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

        [BindProperty]
        public IList<Answers> Answers { get; set; }

        public Users SingleUser { get; set; }

        public List<string> CreatorNames = new List<string>();
        public List<string> CreatorNames2 = new List<string>();
        public List<string> QuestionNames = new List<string>();

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
            List<Users> userlist = await _apiClient.GetUsers();
            List<Questions> qlist = await _apiClient.GetQuestions();
            foreach (Questions q in Questions){
                int userid = q.CreatedBy;
                var tempUser = userlist.Where(e => e.ID == q.CreatedBy).FirstOrDefault();
                SingleUser = tempUser;
                string name = string.Empty;
                if (SingleUser == null || SingleUser.ID == 0)
                {
                    name = "Deleted User";
                }
                else
                {
                    name = SingleUser.FirstName + " " + SingleUser.LastName;
                }
                CreatorNames.Add(name);
            }
            Answers = await _apiClient.GetAnswers();

            foreach (Answers q in Answers)
            {
                int userid = q.CreatedBy;
                var tempUser = userlist.Where(e => e.ID == q.CreatedBy).FirstOrDefault();
                SingleUser = tempUser;
                string name = string.Empty;
                if (SingleUser == null || SingleUser.ID == 0)
                {
                    name = "Deleted User";
                }
                else
                {
                    name = SingleUser.FirstName + " " + SingleUser.LastName;
                }
                CreatorNames2.Add(name);

                var tempqn = qlist.Where(e => e.ID == q.QuestionID).FirstOrDefault();
                string qnname = string.Empty;
                if (tempqn == null || tempqn.ID == 0)
                {
                    qnname = "Deleted Question";
                }
                else
                {
                    qnname = tempqn.Question;
                }
                QuestionNames.Add(qnname);
            }
        }

    }
}
