using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB.Driver;

namespace Web.Entity.Infrastructure.Extensions
{
    public static class MongoCollectionQueryWithOutPageExtensions
    {
        public static async Task<(long totalRecords, IReadOnlyList<TDocument> data)> AggregateWithoutPagination<TDocument>(
            this IMongoCollection<TDocument> collection,
            FilterDefinition<TDocument> filterDefinition,
            SortDefinition<TDocument> sortDefinition)
        {
            // Only count facet is retained as we still might need the total count.
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TDocument>()
                }));
            // Data facet is modified to exclude the skip and limit logic.
            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<TDocument, TDocument>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDefinition)
                }));
            var aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var totalRecords = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<TDocument>();
            return (totalRecords, data);
        }
    }
}
