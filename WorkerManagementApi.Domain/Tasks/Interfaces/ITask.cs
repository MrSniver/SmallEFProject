using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.Common.Entities;
using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Domain.Tasks.Interfaces
{
    public interface ITask
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public TaskDtoStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Guid AssignedTo { get; set; }
        public Guid AssignedBy { get; set; }
    }
}
