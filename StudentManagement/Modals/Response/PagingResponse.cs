﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Modals.Response
{
    public class PagingResponse: BaseResponse
    {
        public PagingInfo PagingInfo { get; set; }
    }
}
