using ProductApi.DAL;
using ProductApi.Services;
using Microsoft.Extensions.Configuration;
using ProductApi.Middleware; // Add this line for your GlobalExceptionMiddleware

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<ProductApi.DAL.IConfigurationProvider, ProductApi.DAL.ConfigurationProvider>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddLogging();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>(); // Ensure this is correct

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
