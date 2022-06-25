namespace TasksListApp.Models.DatabaseSettings
{
    /// <summary>
    /// Settings for connecting to database.
    /// </summary>
    public interface ITaskStoreDatabaseSettings
    {
        string TasksCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
