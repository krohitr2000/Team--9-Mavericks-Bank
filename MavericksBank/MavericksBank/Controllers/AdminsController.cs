using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MavericksBank.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MavericksBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly MavericksBankDb2Context _context;

        public AdminsController(MavericksBankDb2Context context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
          if (_context.Admin == null)
          {
              return NotFound();
          }
            return await _context.Admin.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
          if (_context.Admin == null)
          {
              return NotFound();
          }
            var admin = await _context.Admin.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
          if (_context.Admin == null)
          {
              return Problem("Entity set 'MavericksBankDb2Context.Admin'  is null.");
          }
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.AdminId }, admin);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (_context.Admin == null)
            {
                return NotFound();
            }
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Admins/Login
        [HttpPost("Login")]
        public async Task<ActionResult<Admin>> LoginAdmin(User loginModel)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.Username == loginModel.UserName && a.Password == loginModel.Password);

            if (admin == null)
            {
                return NotFound("Employee not found");
            }

            if (!VerifyPassword(admin, admin.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            return admin;
        }
        private string GenerateJwtToken(Admin admin)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4649167A2A8AFC2B33A8FBC1973E0D2A1D6EFA537A9A5B83D8D9F5A20A3540C00F19A28FBD45FC8E1E95F6C60A5431E4D1F5A1047C487E9C86A7A74F93500C30"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, "Employee")
        };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signinCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            var token = tokenHandler.WriteToken(myToken);

            return token;
        }

        private bool VerifyPassword(Admin admin, string password)
        {
            Console.WriteLine(admin.Username);
            Console.WriteLine(admin.Password);
            return admin.Password == password;
        }


        private bool AdminExists(int id)
        {
            return (_context.Admin?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }
    }
}
