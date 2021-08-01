using System;
using CW.Infrastructure.Interfaces;
using CW.Infrastructure.Models;
using Microsoft.Azure.Documents;

namespace CW.Infrastructure
{
    public class ProductRepository : BaseDbRepository<Product> , IProductRepository
    {
        public ProductRepository(ICosmosDbClientFactory factory) : base(factory) { }

        public override string CollectionName { get; } = "products";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);
    }
}
