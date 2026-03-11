using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Application.Common.Interfaces
{
    public interface IWorkerManApiContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
