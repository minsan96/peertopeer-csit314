using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FrontEnd.Models;
using PeerToPeerDTO;
using Newtonsoft.Json;
using FrontEnd.Services;
using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Pages.QPages
{
    public class DetailsModel : PageModel
    {
        private readonly FrontEnd.Models.QnaContext _context;
        protected readonly IApiClient _apiClient;
        public DetailsModel(FrontEnd.Models.QnaContext context, IApiClient apiClient)
        {
            _context = context;
            _apiClient = apiClient;
        }
        
        [BindProperty]
        public Questions Questions { get; set; }

        public IList<Answers> Answers { get; set; }

        public IList<Comments> Comments { get; set; }

        public List<Users> userlist = new List<Users>();

        public Users _currentUser { get; set; }

        public Answers postanswer = new Answers();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Request.Cookies["CurrentUser"] == null || string.IsNullOrEmpty(Request.Cookies["CurrentUser"]))
            {
                Response.Redirect("/Login");
            }

            Questions = await _context.Questions.FirstOrDefaultAsync(m => m.ID == id);

            if (Questions == null)
            {
                return NotFound();
            }
            else
            {
                Answers = await _apiClient.GetAnswersByQuestionId(Questions.ID);
                Comments = await _apiClient.GetComments();
                userlist = await _apiClient.GetUsers();
                _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
                ViewData["CurrentUserType"] = _currentUser.UserType;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostVotingAsync(int id, string type, string votetype)
        {
            try
            {
                _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
                if (votetype == "down")
                {
                    if (type == "Answer")
                    {
                        var answer = await _apiClient.GetAnswers(id);
                        if (answer.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteAnswers(id, true);
                    }
                    else if (type == "Comment")
                    {
                        var comment = await _apiClient.GetComments(id);
                        if (comment.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteComments(id, true);
                    }
                    else if (type == "Question")
                    {
                        var question = await _apiClient.GetQuestions(id);
                        if (question.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteQuestions(id, true);
                    }
                }
                else
                {
                    if (type == "Answer")
                    {
                        var answer = await _apiClient.GetAnswers(id);
                        if (answer.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteAnswers(id, false);
                    }
                    else if (type == "Comment")
                    {
                        var comment = await _apiClient.GetComments(id);
                        if (comment.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteComments(id, false);
                    }
                    else if (type == "Question")
                    {
                        var question = await _apiClient.GetQuestions(id);
                        if (question.CreatedBy == _currentUser.ID)
                        {
                            return new JsonResult("Error");
                        }
                        var res = _apiClient.UpvoteQuestions(id, false);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            return new JsonResult("voted");
        }

        public async Task<IActionResult> OnPostAnswerAsync(int id, string answertext)
        {
            Questions = await _apiClient.GetQuestions(id);
            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            if (string.IsNullOrEmpty(answertext))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return new JsonResult("Error");
            }
            postanswer.ID = 0;
            postanswer.QuestionID = Questions.ID;
            postanswer.Description = answertext;
            postanswer.CreatedBy = _currentUser.ID;
            postanswer.Rating = 0;

            try
            {
                var post = await _apiClient.PostAnswers(postanswer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            return new JsonResult("Success");
        }

        public async Task<IActionResult> OnPostCommentAsync(int id, string answertext)
        {
            Answers ans = await _apiClient.GetAnswers(id);
            _currentUser = JsonConvert.DeserializeObject<Users>(Request.Cookies["CurrentUser"]);
            Comments postcomment = new Comments();
            if (string.IsNullOrEmpty(answertext))
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return new JsonResult("Error");
            }
            postcomment.ID = 0;
            postcomment.AnswerID = ans.ID;
            postcomment.Description = answertext;
            postcomment.CreatedBy = _currentUser.ID;
            postcomment.Rating = 0;

            try
            {
                var post = await _apiClient.PostComments(postcomment);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return Page();
            }
            return new JsonResult("Success");
        }
    }
}
