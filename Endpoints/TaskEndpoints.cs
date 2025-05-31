using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Endpoints
{
    public static class TaskEndpoints
    {
        public static void RegisterTaskEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/tasks", (TaskService taskService) =>
            {
                var tasks = taskService.GetAllTasks();
                return Results.Ok(tasks);
            });

            endpoints.MapGet("/tasks/{id:int}", (int id, TaskService taskService) =>
            {
                var task = taskService.GetTask(id);
                return task is null ? Results.NotFound() : Results.Ok(task);
            });

            endpoints.MapPost("/tasks", (TaskItem newTask, TaskService taskService) =>
            {
                newTask.CreatedAt = DateTime.UtcNow;
                taskService.CreateTask(newTask);
                return Results.Created($"/tasks/{newTask.Id}", newTask);
            });

            endpoints.MapPut("/tasks/{id:int}", (int id, TaskItem updatedTask, TaskService taskService) =>
            {
                var existingTask = taskService.GetTask(id);
                if (existingTask is null)
                    return Results.NotFound();

                updatedTask.Id = id;
                updatedTask.CreatedAt = existingTask.CreatedAt;

                taskService.UpdateTask(updatedTask);
                return Results.NoContent();
            });

            endpoints.MapDelete("/tasks/{id:int}", (int id, TaskService taskService) =>
            {
                var existingTask = taskService.GetTask(id);
                if (existingTask is null)
                    return Results.NotFound();

                taskService.DeleteTask(id);
                return Results.NoContent();
            });
        }
    }
}
