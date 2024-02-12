using FluentValidation;
using MediatR;
using System.Reflection;
using Tudu.Api.Behaviors;
using Tudu.Api.Middleware;
using Tudu.Api.Shared.Endpoint;
using Tudu.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDateOnlyTimeOnlyStringConverters();
builder.Services.AddSwaggerGen(options =>
{
    options.UseDateOnlyTimeOnlyStringConverters();
});

// DbContext
builder.Services.AddDbContext<TuduDbContext>();

// MediatR and pipeline behavior
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining(typeof(Program)));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetService<TuduDbContext>();
        if(dbContext == null)
        {
            throw new NullReferenceException($"Cannot fetch instance of {nameof(TuduDbContext)} from service provider.");
        }
        await dbContext.Database.EnsureCreatedAsync();
    }

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
        var endpointObject = Activator.CreateInstance(endpointType);
        if(endpointObject == null)
        {
            throw new NullReferenceException($"Cannot create instance of endpoint: {endpointType.Name}.");
        }
        
        IEndpoint endpoint = (IEndpoint)endpointObject;
        endpoint.Map(app);
    }
}