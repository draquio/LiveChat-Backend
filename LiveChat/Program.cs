using LiveChat.Hubs;
using LiveChat.IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InjectDependencies(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors("AllowLocalhost");

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.Run();
