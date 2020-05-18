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
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions()
        {
            return await _context.Questions.ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> GetQuestions(int id)
        {
            var questions = await _context.Questions.FindAsync(id);

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // GET: api/Questions/5
        [HttpGet("{userid}/userid")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestionsByUser(int userid)
        {
            var questions = await _context.Questions.Where(e => e.CreatedBy == userid).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // GET: api/Questions/5
        [HttpGet("{keyword}/search")]
        public async Task<ActionResult<IEnumerable<Questions>>> SearchQuestions(string keyword)
        {
            var questions = await _context.Questions.Where(e => e.Question.Contains(keyword)).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestions(int id, Questions questions)
        {
            if (id != questions.ID)
            {
                return BadRequest();
            }

            var update = await _context.Questions.FindAsync(id);
            update.Question = questions.Question;
            update.Description = questions.Description;
            update.Rating = questions.Rating;
            _context.Entry(update).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(id))
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

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<Questions>> PostQuestions(Questions questions)
        {
            questions.CreatedDate = DateTime.Now;
            _context.Questions.Add(questions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestions", new { id = questions.ID }, questions);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Questions>> DeleteQuestions(int id)
        {
            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(questions);
            await _context.SaveChangesAsync();

            return questions;
        }

        // PUT: api/Questions/5/upvote
        [HttpPut("{id}/{downvote}/upvote")]
        public async Task<IActionResult> UpvoteQuestions(int id, bool downvote)
        {
            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            if (id != questions.ID)
            {
                return BadRequest();
            }
            
            if (downvote)
            {
                questions.Rating = questions.Rating - 1;
            }
            else
            {
                questions.Rating = questions.Rating + 1;
            }
            _context.Entry(questions).State = EntityState.Modified;

            var userid = questions.CreatedBy;
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
                if (!QuestionsExists(id))
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

        // GET: api/Questions/top
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetTop5Questions(int topno = 5, int days = 7)
        {
            var questions = await _context.Questions.Where(e => (DateTime.Now - e.CreatedDate).TotalDays <= days).OrderByDescending(e => e.Rating).Take(topno).ToListAsync();

            if (days == 0)
            {
                questions = await _context.Questions.OrderByDescending(e => e.Rating).Take(topno).ToListAsync();
            }

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        private bool QuestionsExists(int id)
        {
            return _context.Questions.Any(e => e.ID == id);
        }
    }
}
