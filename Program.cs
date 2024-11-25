using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();  
builder.Services.AddSwaggerGen();  

// resolvendo erro de cors.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
        builder.WithOrigins("http://localhost:4200")  
               .AllowAnyMethod()                   
               .AllowAnyHeader());                  
});

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowLocalhost"); 

app.UseSwagger();  
app.UseSwaggerUI(c =>  
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API FAST V1");
});

app.MapControllers();

app.Run();
