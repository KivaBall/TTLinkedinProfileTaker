using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSerilog(config =>
{
    config
        .WriteTo.Console()
        .WriteTo.File("out.log",
            outputTemplate:
            "{Timestamp:dd}.{Timestamp:MM}, {Timestamp:HH:mm:ss} => {Level:u3} | {Message:lj}{NewLine}");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();