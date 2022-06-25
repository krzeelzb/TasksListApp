namespace TasksListApp.Models.DatabaseSettings
{
    public class TaskStoreDatabaseSettings : ITaskStoreDatabaseSettings
    {
        public string TasksCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
