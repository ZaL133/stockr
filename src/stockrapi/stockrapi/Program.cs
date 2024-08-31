
namespace stockrapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var corsPolicyName = "localhost";
            // Add services to the container.
            var apiKey = builder.Configuration["apiKey"];

            builder.Services.AddControllers();
            builder.Services.AddCors(
                options => 
                options.AddPolicy(name: corsPolicyName,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:5173");
                                      policy.WithOrigins("http://localhost:5173");
                                  })
            );
            builder.Services.AddHttpClient();
            builder.Services.AddAutoMapper((x) =>
            {
                
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(corsPolicyName);

            app.MapControllers();

            app.Run();
        }
    }
}