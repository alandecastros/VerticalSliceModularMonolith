using HotChocolate.Data.Filters;
using VerticalSliceModularMonolith.GraphQL.Queries.LivrosOffset;
using VerticalSliceModularMonolith.GraphQL.Queries.UsuariosOffset;
using VerticalSliceModularMonolith.GraphQL.TypeExtensions;
using VerticalSliceModularMonolith.Infrastructure.Database;

namespace VerticalSliceModularMonolith.GraphQL;

public static class ConfigureServices
{
    public static void AddGraphQLConfiguration(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddTypeExtension<UsuarioModelTypeExtension>()
            .AddTypeExtension<LivroModelTypeExtension>()
            .AddAuthorization()
            .RegisterDbContext<AppReadOnlyDbContext>()
            .AddFiltering()
            .AddConvention<IFilterConvention, FilterConventionExtensionForInvariantContainsStrings>()
            .AddProjections()
            .AddSorting()
            .UsePersistedQueryPipeline()
            .AddReadOnlyFileSystemQueryStorage("./GraphQL/PersistedQueries")
            .AddQueryType(q => q.Name("Query"))
            .AddType<UsuariosOffsetQuery>()
            .AddType<LivrosOffsetQuery>();
    }
}
