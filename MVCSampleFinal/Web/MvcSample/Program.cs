
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Automapper;
using Microsoft.Extensions.Logging;

namespace MvcSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddServices();
            builder.Services.AddRepositories(_configuration);
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            // Add automapper
            var mappingConfiguration = new MapperConfiguration (m => m.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfiguration.CreateMapper();
            
            builder.Services.AddSingleton(mapper);

            builder.Services.AddCors(p => p.AddPolicy("CORS_Policy", builder =>
            {
                CorsPolicyBuilder corsPolicyBuilder = builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); //builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddControllersWithViews();
            
            // Configurar sesiones para autenticación
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            // Agregar HttpContextAccessor para acceso a sesión en vistas
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            // Usar sesiones antes de autorización
            app.UseSession();

            app.UseAuthorization();
            app.UseCors("CORS_Policy");
           
            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");
            //app.MapRazorPages();

            // Inicializar usuario administrador si no existe
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    MvcSample.Services.AdminInitializer.InitializeAdmin(context);
                }
                catch (Exception ex)
                {
                    // Log error si es necesario
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error al inicializar el usuario administrador");
                }
            }

            app.Run();
        }
    }
}
