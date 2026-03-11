namespace WorkerManagementApi.Application.ApplicationUsers.Dtos
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Guid ManagerId { get; set; }
        public string Role { get; set; }
    }
}
