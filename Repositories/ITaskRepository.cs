using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using TaskManagementSystem1.Models;

namespace TaskManagementSystem1.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskModel> GetAll(string status = null, string assignedTo = null);
        TaskModel GetById(int id);
        int Add(TaskModel task);        // returns new id
        bool Update(TaskModel task);
        bool SoftDelete(int id);
    }
}