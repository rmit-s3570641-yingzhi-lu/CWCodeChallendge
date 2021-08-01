using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CW.Infrastructure.Exceptions;
using CW.Infrastructure.Interfaces;
using CW.Infrastructure.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CW.Infrastructure
{
    public abstract class BaseDbRepository<T> : IRepository<T>, IProductCollectionContext<T> where T : BaseEntity
    {
        private readonly ICosmosDbClientFactory _cosmosDbClientFactory;

        protected BaseDbRepository(ICosmosDbClientFactory cosmosDbClientFactory)
        {
            _cosmosDbClientFactory = cosmosDbClientFactory;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.ReadDocumentAsync(id, new RequestOptions
                {
                    PartitionKey = ResolvePartitionKey(id)
                });

                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task<List<T>> ListAsync()
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var documents = await cosmosDbClient.ListDocumentAsync();
                var docsToReturn = new List<T>();
                documents.ForEach(doc => 
                {
                    docsToReturn.Add(JsonConvert.DeserializeObject<T>(doc.ToString()));
                });

                return docsToReturn;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.Id = GenerateId(entity);
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.CreateDocumentAsync(entity);
                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new EntityAlreadyExistsException();
                }

                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.ReplaceDocumentAsync(entity.Id, entity);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task DeleteByIdAsync(string id)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.DeleteDocumentAsync(id, new RequestOptions
                {
                    PartitionKey = ResolvePartitionKey(id)
                });
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public abstract string CollectionName { get; }
        public virtual string GenerateId(T entity) => Guid.NewGuid().ToString();
        public virtual PartitionKey ResolvePartitionKey(string entityId) => null;
    }
}
