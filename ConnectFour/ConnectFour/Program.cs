using ConnectFour.DAL.Repositories;
using ConnectFour.DAL.Repositories.Interface;
using ConnectFour.Services;
using ConnectFour.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        //Add Services
        builder.Services.AddScoped<IConnectFourRepository, ConnectFourRepository>();
        builder.Services.AddScoped<IConnectFourService, ConnectFourService>();

        var sqliteCSB = new SqliteConnectionStringBuilder("Data Source=local.db");
        sqliteCSB.DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sqliteCSB.DataSource);
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(sqliteCSB.ToString()));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
        }

        app.Run();
    }
}