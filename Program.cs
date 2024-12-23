using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Project.Models;
using Movie_Project.Data;
using SportsApp.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MovieDbContext>(opts => {
	opts.UseSqlite(
		builder.Configuration.GetConnectionString("MovieDBConnection"));
});

builder.Services.AddDbContext<AppIdentityDbContext>(opts => 
	opts.UseSqlite(
		builder.Configuration["ConnectionStrings:IdentityDBConnection"]));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDbContext>();



var app = builder.Build();




app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
SeedData.EnsurePopulated(app);
await IdentitySeedData.EnsurePopulatedAsync(app);

app.MapStaticAssets();



app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();
	


app.Run();
