using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MavericksBank.Models;

namespace MavericksBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchDetailsController : ControllerBase
    {
        private readonly MavericksBankDb2Context _context;

        public BranchDetailsController(MavericksBankDb2Context context)
        {
            _context = context;
        }

        // GET: api/BranchDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDetails>>> GetBranchDetails()
        {
          if (_context.BranchDetails == null)
          {
              return NotFound();
          }
            return await _context.BranchDetails.ToListAsync();
        }

        // GET: api/BranchDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDetails>> GetBranchDetails(string id)
        {
          if (_context.BranchDetails == null)
          {
              return NotFound();
          }
            var branchDetails = await _context.BranchDetails.FindAsync(id);

            if (branchDetails == null)
            {
                return NotFound();
            }

            return branchDetails;
        }

        // PUT: api/BranchDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranchDetails(string id, BranchDetails branchDetails)
        {
            if (id != branchDetails.IFSCCode)
            {
                return BadRequest();
            }

            _context.Entry(branchDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchDetailsExists(id))
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

        // POST: api/BranchDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BranchDetails>> PostBranchDetails(BranchDetails branchDetails)
        {
          if (_context.BranchDetails == null)
          {
              return Problem("Entity set 'MavericksBankDb2Context.BranchDetails'  is null.");
          }
            _context.BranchDetails.Add(branchDetails);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BranchDetailsExists(branchDetails.IFSCCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBranchDetails", new { id = branchDetails.IFSCCode }, branchDetails);
        }

        // DELETE: api/BranchDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranchDetails(string id)
        {
            if (_context.BranchDetails == null)
            {
                return NotFound();
            }
            var branchDetails = await _context.BranchDetails.FindAsync(id);
            if (branchDetails == null)
            {
                return NotFound();
            }

            _context.BranchDetails.Remove(branchDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BranchDetailsExists(string id)
        {
            return (_context.BranchDetails?.Any(e => e.IFSCCode == id)).GetValueOrDefault();
        }
    }
}
