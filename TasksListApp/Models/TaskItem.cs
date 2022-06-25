using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TasksListApp.Models
{
    [BsonIgnoreExtraElements]
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        [BsonElement("title")]
        public string Title { get; set; } = String.Empty;
        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                $"{nameof(Title)}: {Title}, " +
                $"{nameof(IsCompleted)}: {IsCompleted}";
        }
    }
}
