using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MavericksBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace MavericksBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly MavericksBankDb2Context _context;

        public EmployeesController(MavericksBankDb2Context context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet, Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_context.Employees == null)
          {
              return Problem("Entity set 'MavericksBankDb2Context.Employees'  is null.");
          }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Employees/login
        [HttpPost("login")]
        public async Task<ActionResult<Employee>> Login(EmployeeUser employeeLogin)
        {
            if (string.IsNullOrWhiteSpace(employeeLogin.UserName) || string.IsNullOrWhiteSpace(employeeLogin.Password))
            {
                return BadRequest("Username or password cannot be empty");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == employeeLogin.UserName);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            if (!VerifyPassword(employee, employeeLogin.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateJwtToken(employee);
            employee.Token = token;
            _context.SaveChanges();
            //return Ok(new { Token = token });

            return Ok(new { Employee = employee, Token = token });
        }

        private string GenerateJwtToken(Employee employee)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4649167A2A8AFC2B33A8FBC1973E0D2A1D6EFA537A9A5B83D8D9F5A20A3540C00F19A28FBD45FC8E1E95F6C60A5431E4D1F5A1047C487E9C86A7A74F93500C30"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, employee.Username),
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
        private bool VerifyPassword(Employee employee, string password)
        {
            Console.WriteLine(employee.Username);
            Console.WriteLine(employee.Password);
            return employee.Password == password;
        }
        private string ComputeHash(string password, string salt)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in computedHash)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
