using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        PasswordHasher<Users> hasher = new PasswordHasher<Users>(
            new OptionsWrapper<PasswordHasherOptions>(
                new PasswordHasherOptions()
                {
                    CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                }
            )
        );

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.ID)
            {
                return BadRequest();
            }

            users.Password = GetPassword(id);

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<Users>> CreateUsers(Users users)
        {
            if(UsernameExists(users.UserName))
            {
                return BadRequest("That username is taken. Try another.");
            }

            users.Password = hasher.HashPassword(users, users.Password);
            users.CreatedDate = DateTime.Now;    
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.ID }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        // POST: api/Users/username/password
        [HttpPost("{username}/{password}")]
        public async Task<ActionResult<Users>> Login(string username, string password)
        {
            var users = GetUserByUsername(username);
            if(!UsernameExists(username))
            {
                return NotFound();
            }
            
            if(hasher.VerifyHashedPassword(users, users.Password, password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect password. Try again.");
            }
            users.LastLogin = DateTime.Now;

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return users;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.ID == id);
        }

        private bool UsernameExists(string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }

        private string GetPassword(int id)
        {
            return _context.Users.First(e => e.ID == id).Password;
        }

        private Users GetUserByUsername(string username)
        {
            return _context.Users.First(e => e.UserName == username);
        }
    }
}
