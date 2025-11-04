using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using TaskManagementSystem1.Models;

namespace TaskManagementSystem1.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public IEnumerable<TaskModel> GetAll(string status = null, string assignedTo = null)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Tasks WHERE IsActive = 1";

                if (!string.IsNullOrEmpty(status))
                    sql += " AND Status = @Status";

                if (!string.IsNullOrEmpty(assignedTo))
                    sql += " AND AssignedTo = @AssignedTo";

                sql += " ORDER BY TaskId DESC";

                return con.Query<TaskModel>(sql, new { Status = status, AssignedTo = assignedTo });
            }
        }

        public TaskModel GetById(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                return con.QueryFirstOrDefault<TaskModel>("SELECT * FROM Tasks WHERE TaskId = @id", new { id });
            }
        }

        public int Add(TaskModel model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO Tasks (Title, Description, AssignedTo, DueDate, Status, IsActive, CreatedOn)
                               VALUES (@Title, @Description, @AssignedTo, @DueDate, @Status, 1, GETDATE());
                               SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return con.ExecuteScalar<int>(sql, model);
            }
        }

        public bool Update(TaskModel model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE Tasks SET Title=@Title, Description=@Description,
                               AssignedTo=@AssignedTo, DueDate=@DueDate, Status=@Status
                               WHERE TaskId=@TaskId";
                return con.Execute(sql, model) > 0;
            }
        }

        public bool SoftDelete(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Tasks SET IsActive=0 WHERE TaskId=@id";
                return con.Execute(sql, new { id }) > 0;
            }
        }
    }
}
