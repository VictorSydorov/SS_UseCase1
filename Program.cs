using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR((c)=>c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/countries", async (IMediator mediator, string? name, int? population, string? sort, int? limit) => {
          
	try
	{
        var result = await mediator.Send(new CountryDataRequest(name, population, sort, limit));
        return Results.Ok(result);
    }
	catch (Exception)
	{
        return Results.StatusCode(500);
	}
        
    
});

app.Run();

