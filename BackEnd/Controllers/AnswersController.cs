using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnswersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Answers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answers>>> GetAnswers()
        {
            return await _context.Answers.ToListAsync();
        }

        // GET: api/Answers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Answers>> GetAnswers(int id)
        {
            var answers = await _context.Answers.FindAsync(id);

            if (answers == null)
            {
                return NotFound();
            }

            return answers;
        }

        // GET: api/Answers/5
        [HttpGet("{userid}/userid")]
        public async Task<ActionResult<IEnumerable<Answers>>> GetAnswersByUser(int userid)
        {
            var answers = await _context.Answers.Where(e => e.CreatedBy == userid).ToListAsync();

            if (answers == null)
            {
                return NotFound();
            }

            return answers;
        }

        // GET: api/Answers/5
        [HttpGet("{questionid}/questionid")]
        public async Task<ActionResult<IEnumerable<Answers>>> GetAnswersByQuestion(int questionid)
        {
            var answers = await _context.Answers.Where(e => e.QuestionID == questionid).OrderByDescending(e => e.Rating).ThenBy(e => e.Rating == 0).ToListAsync();

            if (answers == null)
            {
                return NotFound();
            }

            return answers;
        }

        // GET: api/Questions/5
        [HttpGet("{keyword}/search")]
        public async Task<ActionResult<IEnumerable<Answers>>> SearchAnswers(string keyword)
        {
            var answers = await _context.Answers.Where(e => e.Description.Contains(keyword)).ToListAsync();

            if (answers == null)
            {
                return NotFound();
            }

            return answers;
        }

        // PUT: api/Answers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswers(int id, Answers answers)
        {
            if (id != answers.ID)
            {
                return BadRequest();
            }

            var update = await _context.Answers.FindAsync(id);
            update.Description = answers.Description;
            update.Rating = answers.Rating;

            _context.Entry(update).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Answers
        [HttpPost]
        public async Task<ActionResult<Answers>> PostAnswers(Answers answers)
        {
            answers.CreatedDate = DateTime.Now;
            _context.Answers.Add(answers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswers", new { id = answers.ID }, answers);
        }

        // DELETE: api/Answers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answers>> DeleteAnswers(int id)
        {
            var answers = await _context.Answers.FindAsync(id);
            if (answers == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answers);
            await _context.SaveChangesAsync();

            return answers;
        }

        // PUT: api/Answers/5/upvote
        [HttpPut("{id}/{downvote}/upvote")]
        public async Task<IActionResult> UpvoteAnswers(int id, bool downvote)
        {
            var answers = await _context.Answers.FindAsync(id);
            if (answers == null)
            {
                return NotFound();
            }
            if (id != answers.ID)
            {
                return BadRequest();
            }
            
            if (downvote)
            {
                answers.Rating = answers.Rating - 1;
            }
            else
            {
                answers.Rating = answers.Rating + 1;
            }

            _context.Entry(answers).State = EntityState.Modified;

            var userid = answers.CreatedBy;
            var users = await _context.Users.FindAsync(userid);
            if (downvote)
            {
                users.Rating = users.Rating - 1;
            }
            else
            {
                users.Rating = users.Rating + 1;
            }
            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/Answers/top
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Answers>>> GetTop5Answers(int topno = 5, int days = 7)
        {
            var answers = await _context.Answers.Where(e => (DateTime.Now - e.CreatedDate).TotalDays <= days).OrderByDescending(e => e.Rating).Take(topno).ToListAsync();

            if(days == 0)
            {
                answers = await _context.Answers.OrderByDescending(e => e.Rating).Take(topno).ToListAsync();
            }

            if (answers == null)
            {
                return NotFound();
            }

            return answers;
        }

        private bool AnswersExists(int id)
        {
            return _context.Answers.Any(e => e.ID == id);
        }
    }
}
