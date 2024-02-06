namespace VerticalSliceModularMonolith.GraphQL;

public static class ConfigureApplication
{
    public static void UseGraphQL(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapGraphQL();
        }
        else
        {
            app.MapGraphQL().RequireAuthorization();
        }
    }
}