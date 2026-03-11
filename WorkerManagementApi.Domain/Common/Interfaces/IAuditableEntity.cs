namespace WorkerManagementApi.Domain.Common.Interfaces
{
    public interface IAuditableEntity
    {
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}
