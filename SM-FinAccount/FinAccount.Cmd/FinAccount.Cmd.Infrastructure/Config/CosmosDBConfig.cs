namespace Post.Cmd.Infrastructure.Config
{
    public class CosmosDBConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
        public string PartitionKey { get; set; }
    }
}