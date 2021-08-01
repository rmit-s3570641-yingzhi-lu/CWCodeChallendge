namespace CW.Infrastructure
{
    public interface ICosmosDbClientFactory
    {
        ICosmosDbClient GetClient(string collectionName);
    }
}
