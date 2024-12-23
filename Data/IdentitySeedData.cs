using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Project.Data;


namespace SportsApp.Models
{
	public static class IdentitySeedData
	{
		const string berhat = "BerhatYesilyurt";
		const string berhatPassword = "Berhat123$";
		const string berhatMail = "berhat1@gmail.com";
		const string ridvan = "RidvanDursun";
		const string ridvanPassword = "Ridvan123$";
		const string ridvanMail = "ridvan1@gmail.com";

		const string dilan = "DilanAkbulut";
		const string dilanPassword = "Dilan123$";
		const string dilanMail = "dilan1@gmail.com";
		
		  const string semih = "SemihAlben";
		const string semihPassword = "Semih123$";
		const string semihMail = "semih1@gmail.com";

	   
		const string AdminRole = "Admin";
		const string UserRole = "User";
		

		public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (context.Database.GetPendingMigrations().Any())
				{
					context.Database.Migrate();
				}

				if (await roleManager.FindByNameAsync(AdminRole) == null)
				{
					await roleManager.CreateAsync(new IdentityRole(AdminRole));
				}
				if (await roleManager.FindByNameAsync(UserRole) == null)
				{
					await roleManager.CreateAsync(new IdentityRole(UserRole));
				}
				


				var userBerhat = await userManager.FindByNameAsync(berhat);
				if (userBerhat == null)
				{
					userBerhat = new IdentityUser
					{
						UserName = berhat,
						Email = berhatMail
					};

					var result = await userManager.CreateAsync(userBerhat, berhatPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(userBerhat, AdminRole);
					}
					else
					{
						throw new Exception("Users Couldn't save to the Identity Database");
					}
				}

				var userRidvan = await userManager.FindByNameAsync(ridvan);
				if (userRidvan == null)
				{
					userRidvan = new IdentityUser
					{
						UserName = ridvan,
						Email = ridvanMail
					};

					var result = await userManager.CreateAsync(userRidvan, ridvanPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(userRidvan, AdminRole);
					}
					else
					{
						throw new Exception("Users Couldn't save to the Identity Database");
					}
				}

				var userDilan = await userManager.FindByNameAsync(dilan);
				if (userDilan == null)
				{
					userDilan = new IdentityUser
					{
						UserName = dilan,
						Email = dilanMail
					};

					var result = await userManager.CreateAsync(userDilan, dilanPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(userDilan, AdminRole);
					}
					else
					{
						throw new Exception("Users Couldn't save to the Identity Database");
					}
				}
				
				var userSemih = await userManager.FindByNameAsync(semih);
				if (userSemih == null)
				{
					userSemih = new IdentityUser
					{
						UserName = semih,
						Email = semihMail
					};

					var result = await userManager.CreateAsync(userSemih, semihPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(userSemih, AdminRole);
					}
					else
					{
						throw new Exception("Users Couldn't save to the Identity Database");
					}
				}
			}
		}
	}
}
