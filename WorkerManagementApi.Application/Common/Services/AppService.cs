using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;

namespace WorkerManagementApi.Application.Common.Services
{
    public abstract class AppService
    {
        protected IUnitOfWork UnitOfWork { get; }

        public AppService(IUnitOfWork unitOfWork) 
        {
            UnitOfWork = unitOfWork;
        }
    }
}
