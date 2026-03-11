using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.Common.Interfaces;

namespace WorkerManagementApi.Domain.Common.Entities
{
    public abstract class AuditableEntity: Entity, IAuditableEntity
    {
        public AuditableEntity() : base() { }
        public AuditableEntity(Guid id) : base(id) { }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}
