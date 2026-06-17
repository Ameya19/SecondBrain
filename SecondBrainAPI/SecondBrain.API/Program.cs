using Microsoft.EntityFrameworkCore;
using SecondBrain.Core.Interfaces;
using SecondBrain.Infrastructure.Data;
using SecondBrain.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<OllamaEmbeddingService>();
builder.Services.AddHttpClient<OllamaGenerationService>();

builder.Services.AddScoped<IEmbeddingService, OllamaEmbeddingService>();
builder.Services.AddScoped<IIngestionService, IngestionService>();
builder.Services.AddScoped<IGenerationService, OllamaGenerationService>();
builder.Services.AddScoped<IQueryService, QueryService>();

//DBContext
builder.Services.AddDbContext<SecondBrainDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();