using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        TaskEntity Add(TaskEntity task);
        TaskEntity Get(int id);
    }
}
