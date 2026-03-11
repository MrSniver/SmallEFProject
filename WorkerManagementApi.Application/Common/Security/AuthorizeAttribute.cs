using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Application.Common.Security
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute: Attribute
    {

        public AuthorizeAttribute() { }

        public string Roles { get; set; }
        public string Policy { get; set; }
    }
}
