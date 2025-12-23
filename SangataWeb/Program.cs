using Microsoft.EntityFrameworkCore;
using SangataWeb;
using SangataWeb.Class;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped<IGetData, GetData>();
builder.Services.AddScoped<ISetData, SetData>();
builder.Services.AddScoped<IDelData, DelData>();

//var dbSA = "sa";
//var dbHost = "DESKTOP-0VFK7LR";
//var dbName = "SANGATA";
//var dbPassword = "xxxxxx";

var dbSA = "odgadmin";
var dbHost = "tcp:odgerp.database.windows.net";
var dbName = "odg_erp";
var dbPassword = "8lI4392&@*h&£AGuJs/V";

var connectionString = $"Data Source={dbHost},1433;Initial Catalog={dbName};Persist Security Info=True;User ID={dbSA};Password={dbPassword};Connection Timeout=120;TrustServerCertificate=true";
builder.Services.AddDbContext<LightingPlantContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<MinorSystemContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<StoreIDRContext>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(300)));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
