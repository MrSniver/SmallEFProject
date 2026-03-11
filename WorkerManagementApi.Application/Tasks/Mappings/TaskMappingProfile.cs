using AutoMapper;
using WorkerManagementApi.Application.Tasks.Commands.Create;
using WorkerManagementApi.Application.Tasks.Commands.Delete;
using WorkerManagementApi.Application.Tasks.Commands.Update;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Domain.Tasks.Entities;

namespace WorkerManagementApi.Application.Tasks.Mappings
{
    public class TaskMappingProfile: Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<CreateTaskCommand, TaskEntity>();
            CreateMap<DeleteTaskCommand, TaskEntity>();
            CreateMap<UpdateTaskCommand, TaskEntity>();
            CreateMap<TaskEntity, TaskDto>();
            CreateMap<TaskEntity, TaskDto>().ReverseMap();
        }
    }
}
