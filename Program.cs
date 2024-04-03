using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectStudyTool.Data;
using ProjectStudyTool.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// ***** Connect to OpenAI API *****
string _endpoint = System.Configuration.ConfigurationManager.AppSettings["endpoint"]!;
string _apikey = System.Configuration.ConfigurationManager.AppSettings["api-key"]!;
string _deploymentname = System.Configuration.ConfigurationManager.AppSettings["deployment-name"]!;



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = true; //require email confirmation
    options.Stores.MaxLengthForKeys = 128; // increase key for encrypting password
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


// For crud operations
builder.Services.AddScoped<CardService>();

// For razor/blazor components
builder.Services
.AddRazorComponents()
.AddInteractiveServerComponents()
.AddCircuitOptions(options => options.DetailedErrors = true); // for debugging razor components

// Add Radzen services
builder.Services.AddRadzenComponents();

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
app.UseAntiforgery();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//seed data
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();    
    context.Database.Migrate();

    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();  

    IdentitySeedData.Initialize(context, userMgr).Wait();
}

app.Run();
