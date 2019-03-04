using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals.Request
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
