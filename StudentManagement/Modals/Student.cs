using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals
{   [Table("Students")]
    public class Student
    {
        public int StudentID { get; set; }
        public string Code { get; set; }
        [Column("StudentName")]
        [MaxLength(100)]
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public int MajorID { get; set; }
        [ForeignKey("MajorID")]
        public virtual Major Major { get; set; }
    }
}
