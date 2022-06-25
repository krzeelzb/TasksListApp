using MongoDB.Driver;
using TasksListApp.Models;
using TasksListApp.Models.DatabaseSettings;

namespace TasksListApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<TaskItem> tasks;
        private readonly ILogger<TaskService> logger;

        public TaskService(ITaskStoreDatabaseSettings settings, IMongoClient mongoClient,ILogger<TaskService> logger)
        {
            _ = settings ?? throw new ArgumentNullException(nameof(settings));
            _ = mongoClient ?? throw new ArgumentNullException(nameof(mongoClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));


            var database = mongoClient.GetDatabase(settings.DatabaseName);
            tasks = database.GetCollection<TaskItem>(settings.TasksCollectionName);
        }

        public List<TaskItem> GetTasks()
        {
            try
            {
                return tasks.Find(task => true).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while getting list of tasks.", ex);
                throw;
            }
        }

        public TaskItem GetTask(string id)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));

            try
            {
                return tasks.Find(task => task.Id.Equals(id)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while getting task id = {id}.", ex);
                throw;
            }
        }

        public TaskItem CreateTask(TaskItem task)
        {
            _ = task ?? throw new ArgumentNullException(nameof(task));

            try
            {
                tasks.InsertOne(task);
                logger.LogDebug($"New task: {task} successfuly added.");
                return task;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while inserting new task {task}.", ex);
                throw;
            }
        }

        public void UpdateTask(string id, TaskItem task)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));
            _ = task ?? throw new ArgumentNullException(nameof(task));

            try
            {
                tasks.ReplaceOne(task => task.Id.Equals(id), task);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while updating task {task}.", ex);
                throw;
            }
        }

        public void DeleteTask(string id)
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));

            try
            {
                tasks.DeleteOne(task => task.Id.Equals(id));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while deleting task id = {id}.", ex);
                throw;
            }
        }
    }
}
