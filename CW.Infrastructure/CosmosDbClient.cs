using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CW.Infrastructure
{
    public class CosmosDbClient : ICosmosDbClient
    {
        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly IDocumentClient _documentClient;

        public CosmosDbClient(string databaseName, string collectionName, IDocumentClient documentClient)
        {
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            _documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
        }

        public async Task<List<Document>> ListDocumentAsync(FeedOptions options = null, CancellationToken cancellationToken = default)
        {
            var feed = await _documentClient.ReadDocumentFeedAsync(
                        UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), options, cancellationToken);

            var docsToReturn = new List<Document>();

            foreach (Document document in feed)
            {
                docsToReturn.Add(document);
            }

            return docsToReturn;
        }

        public async Task<Document> ReadDocumentAsync(string documentId, RequestOptions options = null,
            CancellationToken cancellationToken = default)
        {
            return await _documentClient.ReadDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentId), options, cancellationToken);
        }

        public async Task<Document> CreateDocumentAsync(object document, RequestOptions options = null,
            bool disableAutomaticIdGeneration = false, CancellationToken cancellationToken = default)
        {
            return await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), document, options,
                disableAutomaticIdGeneration, cancellationToken);
        }

        public async Task<Document> ReplaceDocumentAsync(string documentId, object document,
            RequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentId), document, options,
                cancellationToken);
        }

        public async Task<Document> DeleteDocumentAsync(string documentId, RequestOptions options = null,
            CancellationToken cancellationToken = default)
        {
            return await _documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseName, _collectionName, documentId), options, cancellationToken);
        }
    }
}
