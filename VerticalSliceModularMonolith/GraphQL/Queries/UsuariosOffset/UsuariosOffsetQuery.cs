using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.GraphQL.Queries.UsuariosOffset;

[ExtendObjectType("Query")]
public class UsuariosOffsetQuery
{
    [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 50)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UsuarioModel> UsuariosOffset(AppReadOnlyDbContext dbContext)
    {
        var queryable = dbContext.Set<UsuarioModel>()
            .AsSplitQuery()
            .AsNoTrackingWithIdentityResolution();

        return queryable;
    }
}