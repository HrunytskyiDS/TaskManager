using System.Data.SQLite;
using TaskManager.Models;
using LazyCache;

namespace TaskManager.Services;

public class TaskService
{
    private readonly string _connectionString = "Data Source=taskmanager.db";
    private readonly IAppCache _cache;

    public TaskService(IAppCache cache)
    {
        _cache = cache;
    }

    public IEnumerable<TaskItem> GetAllTasks()
    {
        return _cache.GetOrAdd("all_tasks", () =>
        {
            Console.WriteLine("Отримання даних з БД (не з кешу)");

            var tasks = new List<TaskItem>();

            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tasks";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString()!,
                    Description = reader["Description"]?.ToString(),
                    IsCompleted = Convert.ToBoolean(reader["IsCompleted"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }

            return tasks;
        });
    }

    public TaskItem? GetTask(int id)
    {
        return GetAllTasks().FirstOrDefault(t => t.Id == id);
    }

    public void CreateTask(TaskItem task)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) VALUES (@title, @description, @completed, @created)";
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description ?? "");
        command.Parameters.AddWithValue("@completed", task.IsCompleted);
        command.Parameters.AddWithValue("@created", task.CreatedAt);

        command.ExecuteNonQuery();
        _cache.Remove("all_tasks");
    }

    public void UpdateTask(TaskItem task)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "UPDATE Tasks SET Title = @title, Description = @description, IsCompleted = @completed WHERE Id = @id";
        command.Parameters.AddWithValue("@title", task.Title);
        command.Parameters.AddWithValue("@description", task.Description ?? "");
        command.Parameters.AddWithValue("@completed", task.IsCompleted);
        command.Parameters.AddWithValue("@id", task.Id);

        command.ExecuteNonQuery();
        _cache.Remove("all_tasks");
    }

    public void DeleteTask(int id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Tasks WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
        _cache.Remove("all_tasks");
    }
}
