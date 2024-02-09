using LifeCraft.DataAccess.Data;
using LifeCraft.DataAccess.Repository;
using LifeCraft.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//This service was added when the identity scafollded item was added
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();   // OLD CODE

//Service for adding users and assigning roles
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

//Added, so when we want the implementation in the category controller class, it will know that it will have the immplementation of CategoryRepository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Added because identity uses razor pages instead mvc 
builder.Services.AddRazorPages();

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

app.UseRouting();
//Added this middleware for authentication, must be added before authorization
app.UseAuthentication();
app.UseAuthorization();
//Added for the identity razor pages
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
