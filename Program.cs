using project_veicoli_ado.Dao.Implementations;
using project_veicoli_ado.Dao.Implementations.Tipologie;
using project_veicoli_ado.Dao.Interface;
using project_veicoli_ado.Dao.Interface.Tipologie;
using project_veicoli_ado.Models;
using project_veicoli_ado.Services.Implementations;
using project_veicoli_ado.Services.Interface;
using project_veicoli_ado.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Connessione al database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrazione dei DAO
builder.Services.AddScoped<IBiciDao>(provider => new BiciDao(connectionString));
builder.Services.AddScoped<IMotoDao>(provider => new MotoDao(connectionString));
builder.Services.AddScoped<IMacchinaDao>(provider => new MacchinaDao(connectionString));
builder.Services.AddScoped<ITipoAlimentazioneDao>(provider => new TipoAlimentazioneDao(connectionString));
builder.Services.AddScoped<ICategoriaDao>(provider => new CategoriaDao(connectionString));

// Registrazione dei Service
builder.Services.AddScoped<IVeicoliService<MacchinaViewModel>, MacchinaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
