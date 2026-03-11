using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Application.Common.Exceptions
{
    public class ExceptionResponse: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Description { get; set; }

        public ExceptionResponse() : base() { }

        public ExceptionResponse(HttpStatusCode statusCode, string description) 
        { 
            StatusCode = statusCode;
            Description = description;
        }
    }
}
