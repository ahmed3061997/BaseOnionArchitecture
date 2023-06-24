using API;
using API.Middlewares;
using Application;
using Infrastructure;
using Infrastructure.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandlingMiddleware();

app.MapControllers();
app.MapHub<NotificationHub>("/api/hubs/notifications");

app.Run();
