namespace WorkerManagementApi.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid Id();
        public string Username();
    }
}
