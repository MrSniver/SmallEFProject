namespace WorkerManagementApi.Application.ApplicationUsers.Dtos
{
    public class ApplicationUserFilterDto
    {
        public ApplicationUserFilterDto()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            UserName = string.Empty;
            Role = string.Empty;
            ManagerId = null;
        }

        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? UserName { get; set; }
        public String? Role { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
