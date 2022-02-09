using System;
using System.Collections.Generic;
using System.Text;

namespace PermissionsN5.Core.DTO
{
    public class CustomResponse<T>
    {
        public CustomResponse(T data, string msg)
        {
            Data = data;
            Message = msg;
        }

        public T Data { get; set; }
        public string Message { get; set; }
    }
}
