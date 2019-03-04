using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Modals;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly DB _context;
        public MajorController(DB context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Major>>> Get()
        {
            return await _context.Majors.AsNoTracking().ToListAsync();
        }
       

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Major>> Get(int id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }
            return major;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Major>> Post(Major major)
        {
            var check = await _context.Majors.Where(x => x.MajorID == major.MajorID).FirstOrDefaultAsync();
            if (check != null)
            {
                return BadRequest();
            }
            _context.Majors.Add(major);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = major.MajorID }, major);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Major>> Put(int id, Major major)
        {
            var mj = await _context.Students.FindAsync(id);
            if (mj == null)
            {
                return NotFound();
            }
            mj.StudentID = major.MajorID;
            mj.Name = major.Name;
            _context.Students.Update(mj);
            await _context.SaveChangesAsync();
            return Ok(mj);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Major>> Delete(int id)
        {
            var mj = await _context.Majors.FindAsync(id);
            if (mj == null)
            {
                return NotFound();
            }
            
            _context.Majors.Remove(mj);
            await _context.SaveChangesAsync();
            return mj;
        }
    }
}
