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
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comments>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetComments(int id)
        {
            var comments = await _context.Comments.FindAsync(id);

            if (comments == null)
            {
                return NotFound();
            }

            return comments;
        }

        // GET: api/Comments/5/user
        [HttpGet("{userid}/userid")]
        public async Task<ActionResult<IEnumerable<Comments>>> GetCommentsByUser(int userid)
        {
            var comments = await _context.Comments.Where(e => e.CreatedBy == userid).ToListAsync();

            if (comments == null)
            {
                return NotFound();
            }

            return comments;
        }

        // GET: api/Comments/5/answer
        [HttpGet("{answerid}/answer")]
        public async Task<ActionResult<IEnumerable<Comments>>> GetCommentsByAnswer(int answerid)
        {
            var comments = await _context.Comments.Where(e => e.AnswerID == answerid).ToListAsync();

            if (comments == null)
            {
                return NotFound();
            }

            return comments;
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComments(int id, Comments comments)
        {
            if (id != comments.ID)
            {
                return BadRequest();
            }

            _context.Entry(comments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentsExists(id))
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

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<Comments>> PostComments(Comments comments)
        {
            comments.CreatedDate = DateTime.Now;
            _context.Comments.Add(comments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComments", new { id = comments.ID }, comments);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comments>> DeleteComments(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comments);
            await _context.SaveChangesAsync();

            return comments;
        }

        // PUT: api/Comments/5/upvote
        [HttpPut("{id}/upvote")]
        public async Task<IActionResult> UpvoteComments(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            if (id != comments.ID)
            {
                return BadRequest();
            }

            comments.Rating = comments.Rating + 1;

            _context.Entry(comments).State = EntityState.Modified;

            var userid = comments.CreatedBy;
            var users = await _context.Users.FindAsync(userid);
            users.Rating = users.Rating + 1;
            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentsExists(id))
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

        // GET: api/Comments/top
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Comments>>> GetTop5Comments(int topno = 5, int days = 7)
        {
            var comments = await _context.Comments.Where(e => (DateTime.Now - e.CreatedDate).TotalDays >= 7).OrderByDescending(e => e.Rating).Take(topno).ToListAsync();

            if (days == 0)
            {
                comments = await _context.Comments.OrderByDescending(e => e.Rating).Take(topno).ToListAsync();
            }

            if (comments == null)
            {
                return NotFound();
            }

            return comments;
        }

        private bool CommentsExists(int id)
        {
            return _context.Comments.Any(e => e.ID == id);
        }
    }
}
