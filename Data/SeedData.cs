using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Project.Models;

namespace Movie_Project.Data{
	public static class SeedData{
	   public static void EnsurePopulated(IApplicationBuilder app){

		MovieDbContext context = app.ApplicationServices
			.CreateScope().ServiceProvider.GetRequiredService<MovieDbContext>();


		if(context.Database.GetPendingMigrations().Any()){
			context.Database.Migrate();
		}



		if(!context.Roles.Any()){
			context.Roles.AddRange(
				 new Role{
					RoleName = "Admin"
				},
				new Role{
					RoleName = "User"
				}
			);
			context.SaveChanges();
		}



		if(!context.Categories.Any()){
			context.Categories.AddRange(
				new Category{
					Name = "Advanture"
				},
				new Category{
					Name = "SuperHeros"
				},
				new Category{
					Name = "Crime"
				},
				new Category{
					Name = "Horror"
				},
				new Category{
					Name = "ScienceFiction"
				},
				new Category{
					Name = "Drama"
				},
				new Category{
					Name = "Animation"
				}
			);
			context.SaveChanges();
		}

		

		


		




		Role? adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");

		if (!context.Users.Any())
		{
			if (adminRole != null)
			{
				var passwordCoder = new PasswordHasher<object>(); // PASSWORDLARI VERI TABANIMIZA SIFRELENMIS BIR SEKILDE KAYDETTIM
				context.Users.AddRange(
					new User
					{
						Name = "Berhat",
						Surname = "Yesilyurt",
						Username = "BerhatYesilyurt",
						// Password = passwordCoder.HashPassword(null!,"Serhatturgut1$"),
						Password = "Berhat123$",
						Role = adminRole,
						Email = "berhat1@gmail.com"
					},
					new User{
						Name = "Ridvan",
						Surname = "Dursun",
						Username = "RidvanDursun",
						// Password = passwordCoder.HashPassword(null!,"Serhatturgut1$"),
						Password = "Ridvan123$",
						Role = adminRole,
						Email = "ridvan1@gmail.com"
					},
					 new User{
						Name = "Dilan",
						Surname = "Akbulut",
						Username = "DilanAkbulut",
						// Password = passwordCoder.HashPassword(null!,"Serhatturgut1$"),
						Password = "Dilan123$",
						Role = adminRole,
						Email = "dilan1@gmail.com"
					},
					new User{
                        Name = "Ahmet Semih",
                        Surname = "Alben",
                        Username = "SemihAlben",
                        // Password = passwordCoder.HashPassword(null!,"Serhatturgut1$"),
                        Password = "Semih123$",
                        Role = adminRole,
                        Email = "semih1@gmail.com"
                    }
				);
				context.SaveChanges();
			}
		}
	   

	   
	   }
	}
}