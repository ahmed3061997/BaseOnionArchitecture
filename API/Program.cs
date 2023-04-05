using API;
using API.Middlewares;
using Application;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initialiser.InitialiseAsync();
        //await initialiser.SeedAsync();
    }
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

app.Run();
