using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Modals;
using StudentManagement.Modals.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DB _context;
        public StudentController(DB context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentInfo>>> Get()
        {
            return await _context.Students.Include(x => x.Major).AsNoTracking().Select(x => new StudentInfo {
                    StudentID = x.StudentID,
                    Code = x.Code,
                    Name= x.Name,
                    MajorName=x.Major.Name
                }).ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        //GET 
        [HttpGet("GetByMajorId/{MajorId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetByMajorID(int MajorId)
        {
            return await _context.Students.AsNoTracking().Where(x => x.MajorID==MajorId).ToListAsync();
           
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<Student>> Post(Student student)
        {
            var check = await _context.Students.Where(x => x.StudentID == student.StudentID).FirstOrDefaultAsync();
            if (check != null)
            {
                return BadRequest();
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = student.StudentID }, student);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> Put(int id, Student student)
        {
            var std = await _context.Students.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            std.StudentID = student.StudentID;
            std.Code = student.Code;
            std.Name = student.Name;
            std.MajorID = student.MajorID;
            _context.Students.Update(std);
            await _context.SaveChangesAsync();
            return Ok(std);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> Delete(int id)
        {
            var rec = await _context.Students.FindAsync(id);
            if (rec == null)
            {
                return NotFound();
            }
            _context.Students.Remove(rec);
            await _context.SaveChangesAsync();
            return rec;
        }
    }
}
