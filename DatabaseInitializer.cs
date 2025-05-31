using System.Data.SQLite;
using TaskManager.Models;

namespace TaskManager;

public class DatabaseInitializer
{
    private readonly string _connectionString = "Data Source=taskmanager.db";

    public void Execute()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(*) FROM Tasks";
        var count = Convert.ToInt32(checkCommand.ExecuteScalar());

        if (count == 0)
        {
            var tasks = new List<TaskItem>
            {
                new TaskItem { Title = "Завдання 1", Description = "Опис першого завдання", IsCompleted = false, CreatedAt = DateTime.Now },
                new TaskItem { Title = "Завдання 2", Description = "Опис другого завдання", IsCompleted = true, CreatedAt = DateTime.Now },
                new TaskItem { Title = "Завдання 3", Description = "Опис третього завдання", IsCompleted = false, CreatedAt = DateTime.Now }
            };

            foreach (var task in tasks)
            {
                using var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO Tasks (Title, Description, IsCompleted, CreatedAt) VALUES (@title, @description, @completed, @created)";
                insertCommand.Parameters.AddWithValue("@title", task.Title);
                insertCommand.Parameters.AddWithValue("@description", task.Description ?? "");
                insertCommand.Parameters.AddWithValue("@completed", task.IsCompleted);
                insertCommand.Parameters.AddWithValue("@created", task.CreatedAt);

                insertCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Додано 3 записи до бази даних.");
        }
        else
        {
            Console.WriteLine("У таблиці Tasks вже є дані.");
        }
    }
}
