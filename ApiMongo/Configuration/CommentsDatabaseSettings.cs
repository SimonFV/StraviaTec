namespace ApiMongo.Configuration
{
    public class CommentsDatabaseSettings : ICommentsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CommentsCollectionName { get; set; } = null!;
    }

    public class ICommentsDatabaseSettings
    {
        string ConnectionString { get; set; } = null!;

        string DatabaseName { get; set; } = null!;

        string CommentsCollectionName { get; set; } = null!;
    }
}

