using CW.Infrastructure.Models;
using Microsoft.Azure.Documents;

namespace CW.Infrastructure
{
    public interface IProductCollectionContext<in T> where T : BaseEntity
    {
        string CollectionName { get; }

        string GenerateId(T entity);

        PartitionKey ResolvePartitionKey(string entityId);
    }
}
