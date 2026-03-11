using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.Common.Interfaces;

namespace WorkerManagementApi.Domain.Common.Entities
{
    public class Entity: IEntity
    {
        public Entity()
        {

        }

        public Entity(Guid id)
        {

        }

        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
