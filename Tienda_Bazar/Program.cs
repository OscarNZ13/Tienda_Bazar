using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexión para el DbContext
builder.Services.AddDbContext<BazarLibreriaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BazarLibreriaConnection")));

// Agrega los servicios necesarios para el contenedor
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor predeterminado de HSTS es 30 días. Puedes cambiarlo para escenarios de producción. Más información en https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Paginas}/{action=Bienvenida}/{id?}");

app.Run();