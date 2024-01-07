using System.Reflection;
using Tudu.Api.Shared.Endpoint;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

MapEndpoints();

app.UseHttpsRedirection();

app.Run();

void MapEndpoints()
{
    var endpointTypes = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && typeof(IEndpoint).IsAssignableFrom(type))
        .ToArray();

    foreach (var endpointType in endpointTypes)
    {
        IEndpoint endpoint = (IEndpoint)Activator.CreateInstance(endpointType);
        endpoint.Map(app);
    }
}