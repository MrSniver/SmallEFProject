using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Application.Tasks.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TaskDtoStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Guid AssignedTo { get; set; }
        public Guid AssignedBy { get; set; }
    }
}
