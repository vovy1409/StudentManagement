using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals.Response
{
    public class BaseResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public BaseResponse() {}
        public BaseResponse(object data)
        {
            this.Data = data;

        }
}
}
