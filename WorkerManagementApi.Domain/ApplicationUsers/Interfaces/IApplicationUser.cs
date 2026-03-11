namespace WorkerManagementApi.Domain.ApplicationUsers.Interfaces
{
    public interface IApplicationUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? ManagerId { get; set; }
        public string UserName { get; set; }
    }
}
