using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MavericksBank.Models;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;


namespace MavericksBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        

        private readonly MavericksBankDb2Context _context;

        public CustomersController(MavericksBankDb2Context context)
        {
            _context = context;
            
        }

        // GET: api/Customers
        [HttpGet, Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
          if (_context.Customers == null)
          {
              return Problem("Entity set 'MavericksBankDb2Context.Customers'  is null.");
          }

            var token = GenerateJwtToken(customer);

            customer.Token = token;


            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Customers/login
        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login(User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Email or password cannot be empty");
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == user.UserName);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            if (!VerifyPassword(customer, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtToken(customer);

            customer.Token = token;

            _context.SaveChanges();

            //return Ok(new { Token = token });
            return customer;
        }

        // POST: api/Customers/verify
        [HttpPost("verify")]
        public async Task<ActionResult<Customer>> VerifyCustomerDetails(Customer customerDetails)
        {
            if (string.IsNullOrWhiteSpace(customerDetails.Username) ||
                string.IsNullOrWhiteSpace(customerDetails.Name) ||
                string.IsNullOrWhiteSpace(customerDetails.Email) ||
                string.IsNullOrWhiteSpace(customerDetails.Address))
            {
                return BadRequest("Username, name, email, and location cannot be empty");
            }

            // Find customer by details
            var customer = await _context.Customers.FirstOrDefaultAsync(c =>
                c.Username == customerDetails.Username &&
                c.Name == customerDetails.Name &&
                c.Email == customerDetails.Email &&
                c.Address == customerDetails.Address);

            if (customer == null)
            {
                return NotFound("Customer not found with provided details");
            }

            return customer;
        }


        private bool VerifyPassword(Customer customer, string password)
        {
            Console.WriteLine(customer.Username);
            Console.WriteLine(customer.Password);
            return customer.Password == password;
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

        private string GenerateJwtToken(Customer customer)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4649167A2A8AFC2B33A8FBC1973E0D2A1D6EFA537A9A5B83D8D9F5A20A3540C00F19A28FBD45FC8E1E95F6C60A5431E4D1F5A1047C487E9C86A7A74F93500C30"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.Username),
                    new Claim(ClaimTypes.Role, "Customer")
                };

            //var tokeOptions = new JwtSecurityToken(
            //    issuer: "http://localhost:5126/",
            //    audience: "http://localhost:5126/",
            //    claims: claims,
            //    expires: DateTime.Now.AddDays(1),
            //    signingCredentials: signinCredentials
            //);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = signinCredentials
            };
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            //return tokenString;

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            var token = tokenHandler.WriteToken(myToken);

            return token;
        }


        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
