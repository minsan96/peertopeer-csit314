using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PeerToPeerDTO;

namespace FrontEnd.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Questions
        public async Task<List<Questions>> GetQuestions()
        {
            var response = await _httpClient.GetAsync("/api/Questions");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Questions>>();
        }

        public async Task<Questions> GetQuestions(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Questions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Questions>();
        }

        public async Task PutQuestions(int id, Questions questions)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Questions/{questions.ID}", questions);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> PostQuestions(Questions questions)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Questions", questions);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task DeleteQuestions(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Questions/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Questions>> GetQuestionsByUserId(int userid)
        {
            var response = await _httpClient.GetAsync($"/api/Questions/{userid}/userid");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Questions>>();
        }

        public async Task<List<Questions>> SearchQuestions(string keyword)
        {
            var response = await _httpClient.GetAsync($"/api/Questions/{keyword}/search");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Questions>>();
        }

        public async Task UpvoteQuestions(int id)
        {
            var response = await _httpClient.PutAsync($"/api/Questions/{id}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Questions>> GetTopRatedQuestions(int top = 5, int days = 7)
        {
            var response = await _httpClient.GetAsync($"/api/Questions/top?topno={top}&days={days}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Questions>>();
        }
        #endregion

        #region Answers
        public async Task<List<Answers>> GetAnswers()
        {
            var response = await _httpClient.GetAsync("/api/Answers");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Answers>>();
        }

        public async Task<Answers> GetAnswers(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Answers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Answers>();
        }

        public async Task PutAnswers(int id, Answers answers)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Answers/{answers.ID}", answers);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> PostAnswers(Answers answers)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Answers", answers);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task DeleteAnswers(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Answers/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Answers>> GetAnswersByUserId(int userid)
        {
            var response = await _httpClient.GetAsync($"/api/Answers/{userid}/userid");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Answers>>();
        }

        public async Task<List<Answers>> GetAnswersByQuestionId(int questionid)
        {
            var response = await _httpClient.GetAsync($"/api/Answers/{questionid}/questionid");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Answers>>();
        }

        public async Task<List<Answers>> SearchAnswers(string keyword)
        {
            var response = await _httpClient.GetAsync($"/api/Answers/{keyword}/search");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Answers>>();
        }

        public async Task UpvoteAnswers(int id)
        {
            var response = await _httpClient.PutAsync($"/api/Answers/{id}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Answers>> GetTopRatedAnswers(int top = 5, int days = 7)
        {
            var response = await _httpClient.GetAsync($"/api/Answers/top?topno={top}&days={days}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Answers>>();
        }
        #endregion

        #region Comments
        public async Task<List<Comments>> GetComments()
        {
            var response = await _httpClient.GetAsync("/api/Comments");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Comments>>();
        }

        public async Task<Comments> GetComments(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Comments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Comments>();
        }

        public async Task PutComments(int id, Comments comments)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Comments/{comments.ID}", comments);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> PostComments(Comments comments)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Comments", comments);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task DeleteComments(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Comments/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Comments>> GetCommentsByUserId(int userid)
        {
            var response = await _httpClient.GetAsync($"/api/Comments/{userid}/userid");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Comments>>();
        }

        public async Task<List<Comments>> GetCommentsByAnswerId(int answerid)
        {
            var response = await _httpClient.GetAsync($"/api/Comments/{answerid}/answer");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Comments>>();
        }

        public async Task<List<Comments>> SearchComments(string keyword)
        {
            var response = await _httpClient.GetAsync($"/api/Comments/{keyword}/search");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Comments>>();
        }

        public async Task UpvoteComments(int id)
        {
            var response = await _httpClient.PutAsync($"/api/Comments/{id}", null);

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Comments>> GetTopRatedComments(int top = 5, int days = 7)
        {
            var response = await _httpClient.GetAsync($"/api/Comments/top?topno={top}&days={days}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Comments>>();
        }
        #endregion

        #region Users
        public async Task<List<Users>> GetUsers()
        {
            var response = await _httpClient.GetAsync("/api/Users");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Users>>();
        }

        public async Task<Users> GetUsers(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Users/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Users>();
        }

        public async Task PutUsers(int id, Users users)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Users/{users.ID}", users);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> CreateUsers(Users users)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/Users", users);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task DeleteUsers(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Users/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        public async Task<Users> Login(string username, string password)
        {
            var response = await _httpClient.PostAsync($"/api/Users/{username}/{password}", null);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<Users>();
        }

        public async Task<List<Users>> GetTopRatedUsers(int top = 5, int days = 7)
        {
            var response = await _httpClient.GetAsync($"/api/Users/top?topno={top}&days={days}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Users>>();
        }
        #endregion
    }
}
