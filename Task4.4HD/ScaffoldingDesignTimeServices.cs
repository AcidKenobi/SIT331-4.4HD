using EntityFrameworkCore.Scaffolding.Handlebars;
using HandlebarsDotNet;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Design;

public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        services.AddHandlebarsScaffolding(options =>
        {
            options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
            options.ExcludedTables = new List<string> { "users" };

        });

        services.AddHandlebarsTransformers2(
            entityTypeNameTransformer: name => name.Singularize().Pascalize(),      // Class name
            entityFileNameTransformer: name => name.Singularize().Pascalize(),      // File name
            propertyTransformer: (entity, prop) => new EntityPropertyInfo(
                prop.PropertyType,
                prop.PropertyName.Pascalize(),                                      // Property name
                prop.PropertyIsNullable),

            navPropertyTransformer: (entity, prop) => new EntityPropertyInfo(
                prop.PropertyType,
                prop.PropertyName.Pascalize())                                      // Nav prop
        );

    }

    
}

//Tutorial 
//-- Command to template
//dotnet ef dbcontext scaffold 
//-- SQL database connection string
//"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" 
//-- Provider
//Microsoft.EntityFrameworkCore.SqlServer -o Models -c 
//NorthwindSlimContext -f --context-dir Contexts

//Ours
// dotnet ef dbcontext scaffold 
// "Host=localhost;Database=sit331;Username=postgres;Password=password"
// Npgsql.EntityFrameworkCore.PostgreSQL 
//** Put context in Persistence folder, models in Models folder. Name RobotContext
//--context-dir Persistence --output-dir Models --context templateTest 

//dotnet ef dbcontext scaffold "Host=localhost;Database=sit331;Username=postgres;Password=password" Npgsql.EntityFrameworkCore.PostgreSQL --context-dir Persistence --output-dir Models --context RobotContext --force --no-pluralize
