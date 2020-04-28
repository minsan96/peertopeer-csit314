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
        #endregion

        #region Answers
        Task<List<Answers>> GetAnswers();

        Task<Answers> GetAnswers(int id);

        Task PutAnswers(int id, Answers users);

        Task<bool> PostAnswers(Answers users);

        Task DeleteAnswers(int id);
        #endregion

        #region Comments
        Task<List<Comments>> GetComments();

        Task<Comments> GetComments(int id);

        Task PutComments(int id, Comments comments);

        Task<bool> PostComments(Comments comments);

        Task DeleteComments(int id);
        #endregion

        #region Users
        Task<List<Users>> GetUsers();

        Task<Users> GetUsers(int id);

        Task PutUsers(int id, Users users);

        Task<bool> CreateUsers(Users users);

        Task DeleteUsers(int id);

        Task<Users> Login(string username, string password);
        #endregion
    }
}
