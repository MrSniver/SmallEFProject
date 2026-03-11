using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Application.Tasks.Dtos
{
    public class TaskFilterDto
    {
        public TaskFilterDto()
        {
            Status = null;
            CreatedDate = null;
            FinishedDate = null;
            AssignedTo = null;
            AssignedBy = null;
        }

        public TaskDtoStatus? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
    }
}
