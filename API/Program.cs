using API.Cache;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var configuration =  builder.Configuration;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(configuration.GetValue<string>("Redis")!, Console.Out));
builder.Services.AddSingleton<ICacheService, RedisCacheService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/get/{key}", async ([FromRoute]string key, ICacheService cacheService) => await cacheService.TryGetAsync<string>(key))
.WithName("Get from cache")
.WithOpenApi();

app.MapPost("/set", async (CacheRequest cacheRequest, ICacheService cacheService) => await cacheService.UpsertAsync(cacheRequest.Key, cacheRequest.Value))
.WithName("Add to cache")
.WithOpenApi();

app.Run();
