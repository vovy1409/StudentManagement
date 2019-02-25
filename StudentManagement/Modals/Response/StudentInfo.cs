using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals.Response
{
    public class StudentInfo
    {
        public int StudentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MajorName { get; set; }
    }
}
