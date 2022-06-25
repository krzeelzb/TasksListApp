using TasksListApp.Models;

namespace TasksListApp.Services
{
    /// <summary>
    /// Service that connects to database and executes CRUD operations
    /// </summary>
    public interface ITaskService
    {
        List<TaskItem> GetTasks();
        TaskItem GetTask(string id);
        TaskItem CreateTask(TaskItem task);
        void UpdateTask(string id, TaskItem task);
        void DeleteTask(string id);
    }
}
