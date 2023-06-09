using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCAngularShortener.Data;
using MVCAngularShortener.Data.Interfaces;
using MVCAngularShortener.Infrastructure;
using MVCAngularShortener.Infrastructure.Interfaces;
using MVCAngularShortener.Infrastructure.Repository;
using MVCAngularShortener.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUrlsRepository, UrlRepository>();
builder.Services.AddTransient<IShortener, UrlShortenerService>();


builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Url}/{action=Index}/{id?}");
app.MapRazorPages();
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
app.Run();
