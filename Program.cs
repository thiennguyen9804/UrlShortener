
using IdGen;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Context;
using UrlShortener.Utils;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            var fixedEpoch = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.Services.AddSingleton<IIdGenerator<long>>(
                new Snowflake41BitIdGenerator(generatorId: 0, fixedEpoch: fixedEpoch)
            );

            builder.Services.AddControllers();
            builder.Services.AddDbContext<UrlContext>(options => options.UseSqlite(connString));
            var app = builder.Build();
            app.MapControllers();

            app.Run();
        }
    }
}
