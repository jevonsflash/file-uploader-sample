var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "default",
                      policy =>
                      {
                          policy.AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin();
                      });
});
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("default");


// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.MapGet("/readme", () => { return "Hello world"; });

app.Run();
