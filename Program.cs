using TestBackTickNSwag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureServicesX();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMethods("patch1", new string[] { "PATCH" },
    delegate(MyGenericType<WeatherForecast> data, HttpContext context)
    {
        Console.WriteLine(data.Foo);
    });


app.MapMethods("patch2", new string[] { "PATCH" },
    delegate(WeatherForecast data, HttpContext context)
    {
        Console.WriteLine(data);
    });

app.Run();
