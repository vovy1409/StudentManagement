using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals.Response
{
    public class LoginResponse
    {

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
