using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
public class Program
{
    public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();

    // EF Core uses this method at design time to access the DbContext
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder.UseStartup<Startup>());
}
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    //builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
        });

        // Other service configurations...
        services.AddControllers();

        services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Presentation")));

        // services.AddDbContext<ApplicationDbContext>(
        //     options => options.UseNpgsql("name=ConnectionStrings:DefaultConnection"));

        // services.AddIdentity<Player, IdentityRole<int>>()
        // .AddEntityFrameworkStores<ApplicationDbContext>()
        // .AddDefaultTokenProviders();

        services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<RoleManager<IdentityRole<int>>>();
        services.AddScoped<RoleService>();

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITournamentService, TournamentService>();
        services.AddScoped<ITournamentRepository, TournamentRepository>();
        services.AddScoped<ITournamentPlayerService, TournamentPlayerService>();
        services.AddScoped<ITournamentPlayerRepository, TournamentPlayerRepository>();
        services.AddScoped<ITournamentMatchService, TournamentMatchService>();
        services.AddScoped<ITournamentMatchRepository, TournamentMatchRepository>();
        services.AddScoped<IDeckService, DeckService>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();

    var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
    services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

        // services.AddIdentity<Player, IdentityRole>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>()
        //     .AddDefaultTokenProviders();

        // var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);
        // services.AddAuthentication(x =>
        // {
        //     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // })
        // .AddJwtBearer(x =>
        // {
        //     x.RequireHttpsMetadata = false;
        //     x.SaveToken = true;
        //     x.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuerSigningKey = true,
        //         IssuerSigningKey = new SymmetricSecurityKey(key),
        //         ValidateIssuer = false,
        //         ValidateAudience = false
        //     };
        // });
    }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context, RoleService roleService)
        {
        app.UseCors("AllowSpecificOrigin");
        //app.UseCors("AllowAllOrigins");
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseStaticFiles();
        
        app.UseAuthentication(); // This should come before UseAuthorization
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        // Seed data
        SeedData(context);

        context.Database.Migrate();  // Ensure the database is created and all migrations are applied
        roleService.CreateRoles().Wait();  // Now you can create the roles
        // Create roles on startup
        // CreateRoles(roleManager).Wait();
    }


private async void SeedData(ApplicationDbContext context)
{
    // Check if data is already seeded
    if (context.Provinces.Any())
    {
        return; // Data was already seeded
    }

   // Add provinces and municipalities
var provinces = new Dictionary<string, List<string>>()
{
    {"Pinar del Río", new List<string>{"Consolación del Sur", "Guane", "La Palma", "Los Palacios", "Mantua", "Minas de Matahambre", "Pinar del Río", "San Juan y Martínez", "San Luis", "Sandino", "Viñales"}},
    {"Artemisa", new List<string>{"Alquízar", "Artemisa", "Bauta", "Caimito", "Guanajay", "Güira de Melena", "Mariel", "San Antonio de los Baños", "Bahía Honda", "Candelaria", "San Cristóbal"}},
    {"Mayabeque", new List<string>{"Batabanó", "Bejucal", "Güines", "Jaruco", "Madruga", "Melena del Sur", "Nueva Paz", "Quivicán", "San José de las Lajas", "San Nicolás de Bari", "Santa Cruz del Norte"}},
    {"La Habana", new List<string>{"Arroyo Naranjo", "Boyeros", "Centro Habana", "Cerro", "Cotorro", "Diez de Octubre", "Guanabacoa", "Habana del Este", "Habana Vieja", "La Lisa", "Marianao", "Playa", "Plaza de la Revolución", "Regla", "San Miguel del Padrón"}},
    {"Matanzas", new List<string>{"Calimete", "Cárdenas", "Ciénaga de Zapata", "Colón", "Jagüey Grande", "Jovellanos", "Limonar", "Los Arabos", "Martí", "Matanzas", "Pedro Betancourt", "Perico", "Unión de Reyes"}},
    {"Cienfuegos", new List<string>{"Abreus", "Aguada de Pasajeros", "Cienfuegos", "Cruces", "Cumanayagua", "Palmira", "Rodas", "Santa Isabel de las Lajas"}},
    {"Villa Clara", new List<string>{"Caibarien", "Camajuaní", "Cifuentes", "Corralillo", "Encrucijada", "Manicaragua", "Placetas", "Quemado de Güines", "Ranchuelo", "Remedios", "Sagua la Grande", "Santa Clara", "Santo Domingo"}},
    {"Sancti Spíritus", new List<string>{"Cabaiguán", "Fomento", "Jatibonico", "La Sierpe", "Sancti Spíritus", "Taguasco", "Trinidad", "Yaguajay"}},
    {"Ciego de Ávila", new List<string>{"Ciro Redondo (Municipio)", "Baraguá", "Bolivia", "Chambas", "Ciego de Ávila", "Florencia", "Majagua", "Morón", "Primero de Enero", "Venezuela"}},
    {"Camagüey", new List<string>{"Camagüey", "Carlos Manuel de Céspedes", "Esmeralda", "Florida", "Guaimaro", "Jimagüayú", "Minas", "Najasa", "Nuevitas", "Santa Cruz del Sur", "Sibanicú", "Sierra de Cubitas", "Vertientes"}},
    {"Las Tunas", new List<string>{"Amancio Rodríguez", "Colombia", "Jesús Menéndez", "Jobabo", "Las Tunas", "Majibacoa", "Manatí", "Puerto Padre"}},
    {"Holguín", new List<string>{"Antilla", "Báguanos", "Banes", "Cacocum", "Calixto García", "Cueto", "Frank País", "Gibara", "Holguín", "Mayarí", "Moa", "Rafael Freyre", "Sagua de Tánamo", "Urbano Noris"}},
    {"Granma", new List<string>{"Bartolomé Masó", "Bayamo", "Buey Arriba", "Campechuela", "Cauto Cristo", "Guisa", "Jiguaní", "Manzanillo", "Media Luna", "Niquero", "Pilón", "Río Cauto", "Yara"}},
    {"Santiago de Cuba", new List<string>{"Contramaestre", "Guamá", "Julio Antonio Mella", "Palma Soriano", "San Luis", "Santiago de Cuba", "Segundo Frente", "Songo la Maya", "Tercer Frente"}},
    {"Guantánamo", new List<string>{"Baracoa", "Caimanera", "El Salvador", "Guantánamo", "Imías", "Maisí", "Manuel Tames", "Niceto Pérez", "San Antonio del Sur", "Yateras"}},
    {"Municipio Especial Isla de la Juventud", new List<string>{}}
};


            foreach (var provinceEntry in provinces)
            {
                var province = new Province { ProvinceName = provinceEntry.Key };
                context.Provinces.Add(province);

                foreach (var municipalityName in provinceEntry.Value)
                {
                    var municipality = new Municipality { Name = municipalityName, Province = province };
                    context.Municipalities.Add(municipality);
                }
            }

        
            context.SaveChanges();
            Console.WriteLine("Seed data added successfully.");
}


private async Task CreateRoles(RoleManager<IdentityRole<int>> roleManager)
{
    // Add a check to create the role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>{ Name = "Administrator" });
    }

    if (!await roleManager.RoleExistsAsync("Player"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>{ Name = "Player" });
    }

    if (!await roleManager.RoleExistsAsync("SuperAdministrator"))
    {
        await roleManager.CreateAsync(new IdentityRole<int>{ Name = "SuperAdministrator" });
    }
}

}
