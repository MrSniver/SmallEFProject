using AutoMapper;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Create;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Delete;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Update;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Mappings
{
    public class ApplicationUserMappingProfile: Profile
    {
        public ApplicationUserMappingProfile() 
        {
            CreateMap<CreateApplicationUserCommand, ApplicationUser>();
            CreateMap<DeleteApplicationUserCommand, ApplicationUser>();
            CreateMap<UpdateApplicationUserCommand, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(x => x.Role, x => x.MapFrom(y => y.Roles.Select(x => x.Role).FirstOrDefault()));
        }
    }
}
