using Microsoft.OData.Json;

namespace ODataSkipTokenException;

using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using ODataSkipTokenException.Model;


public class Program
{
    private const string Reviews = "reviews";
    private const string Books = "books";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var modelBuilder = new ODataConventionModelBuilder();
        var books = modelBuilder.EntitySet<Book>(Books);
        modelBuilder.EntitySet<Review>(Reviews);
        books.HasManyBinding(b => b.Reviews, Reviews);
        modelBuilder.EnableLowerCamelCase();

        builder.Services.AddControllers().AddOData(
            options =>
            {
                options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).SkipToken().AddRouteComponents(
                    "odata",
                    modelBuilder.GetEdmModel(), services =>
                    {
                        services.AddSingleton<IStreamBasedJsonWriterFactory>(_ =>
                            DefaultStreamBasedJsonWriterFactory.Default);
                    });
                options.RouteOptions.EnableKeyInParenthesis = false;
                options.RouteOptions.EnablePropertyNameCaseInsensitive = true;
                options.RouteOptions.EnableControllerNameCaseInsensitive = true;
                options.RouteOptions.EnableActionNameCaseInsensitive = true;
                
                
            });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseODataRouteDebug("odata/routes");
        }


        app.UseODataQueryRequest();
        app.MapControllers();
        app.Run();
    }
}