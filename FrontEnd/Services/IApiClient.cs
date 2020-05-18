using System.Collections.Generic;
using System.Threading.Tasks;
using PeerToPeerDTO;

namespace FrontEnd.Services
{
    public interface IApiClient
    {
        #region Questions
        Task<List<Questions>> GetQuestions();

        Task<Questions> GetQuestions(int id);

        Task PutQuestions(int id, Questions users);

        Task<bool> PostQuestions(Questions users);

        Task DeleteQuestions(int id);

        Task<List<Questions>> GetQuestionsByUserId(int userid);

        Task<List<Questions>> SearchQuestions(string keyword);

        Task UpvoteQuestions(int id, bool downvote = false);

        Task<List<Questions>> GetTopRatedQuestions(int top = 5, int days = 7);
        #endregion

        #region Answers
        Task<List<Answers>> GetAnswers();

        Task<Answers> GetAnswers(int id);

        Task PutAnswers(int id, Answers users);

        Task<bool> PostAnswers(Answers users);

        Task DeleteAnswers(int id);

        Task<List<Answers>> GetAnswersByUserId(int userid);

        Task<List<Answers>> GetAnswersByQuestionId(int questionid);

        Task<List<Answers>> SearchAnswers(string keyword);

        Task UpvoteAnswers(int id, bool downvote = false);

        Task<List<Answers>> GetTopRatedAnswers(int top = 5, int days = 7);
        #endregion

        #region Comments
        Task<List<Comments>> GetComments();

        Task<Comments> GetComments(int id);

        Task PutComments(int id, Comments comments);

        Task<bool> PostComments(Comments comments);

        Task DeleteComments(int id);

        Task<List<Comments>> GetCommentsByUserId(int userid);

        Task<List<Comments>> GetCommentsByAnswerId(int answerid);

        Task<List<Comments>> SearchComments(string keyword);

        Task UpvoteComments(int id, bool downvote = false);

        Task<List<Comments>> GetTopRatedComments(int top = 5, int days = 7);
        #endregion

        #region Users
        Task<List<Users>> GetUsers();

        Task<Users> GetUsers(int id);

        Task PutUsers(int id, Users users);

        Task<bool> CreateUsers(Users users);

        Task DeleteUsers(int id);

        Task<Users> Login(string username, string password);

        Task<List<Users>> GetTopRatedUsers(int top = 5, int days = 7);

        Task ChangePassword(int id, string password);
        #endregion
    }
}
