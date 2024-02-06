using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.GraphQL.Queries.LivrosOffset;

[ExtendObjectType("Query")]
public class LivrosOffsetQuery
{
    [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 50)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<LivroModel> LivrosOffset(AppReadOnlyDbContext dbContext)
    {
        var queryable = dbContext.Set<LivroModel>()
            .AsSplitQuery()
            .AsNoTrackingWithIdentityResolution();

        return queryable;
    }
}
