using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
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
        private readonly IHostingEnvironment _hostingEnvironment;
        public StudentController(IHostingEnvironment hostingEnvironment, DB context)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<ActionResult<Student>> Post([FromForm] Student student)
        {
            var check = await _context.Students.Where(x => x.StudentID == student.StudentID).FirstOrDefaultAsync();
            if (check != null)
            {
                return BadRequest();
            }
            if (await _context.Majors.FindAsync(student.MajorID) == null)
            {
                return BadRequest("Khong ton tai major id");
            }
            
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var file = student.File;
            if (file != null)
            {
                string newFileName = student.StudentID + "_" + file.FileName;
                string path = _hostingEnvironment.ContentRootPath + "\\Data\\" + newFileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    student.ImagePath = newFileName;
                    _context.Entry(student).Property(x => x.ImagePath).IsModified = true;
                    _context.SaveChanges();
                }
                
            }
            student.File = null;
            return CreatedAtAction("Get", new { id = student.StudentID }, student);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> Put(int id, [FromForm] Student student)
        {
            var std = await _context.Students.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            std.Code = student.Code;
            std.Name = student.Name;
            std.MajorID = student.MajorID;
            var file = student.File;
            if (file != null)
            {
                string path = _hostingEnvironment.ContentRootPath + "\\Data\\" + std.ImagePath;
                if ((System.IO.File.Exists(path)))
                {
                    System.IO.File.Delete(path);
                }
                string newFileName = std.StudentID + "_" + file.FileName;
                string path1 = _hostingEnvironment.ContentRootPath + "\\Data\\" + newFileName;
                using (var stream = new FileStream(path1, FileMode.Create))
                {
                    file.CopyTo(stream);
                    std.ImagePath = newFileName;
                }

            }
            _context.Students.Update(std);
            await _context.SaveChangesAsync();
            return Ok(std);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> Delete(int id)
        {
            var std = await _context.Students.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            string path = _hostingEnvironment.ContentRootPath + "\\Data\\" + std.ImagePath;
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
            }
            _context.Students.Remove(std);
            await _context.SaveChangesAsync();
            return std;
        }
    }
}
