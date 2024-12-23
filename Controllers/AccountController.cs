using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Project;
using Movie_Project.Models;
using Movie_Project.ViewModels;

public class AccountController:Controller{
	private readonly SignInManager<IdentityUser> _signInManager;  
	private readonly UserManager<IdentityUser> _userManager;

	private readonly MovieDbContext _context;
	public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,MovieDbContext movieDbContext){
		_signInManager = signInManager;
		_userManager = userManager;
		_context = movieDbContext;
	}
	[HttpGet]
	public IActionResult Login(){
	   return View();
	}
	 [HttpPost]
		public async Task<IActionResult> Login(LoginUserView userToLogin)
{
	// ModelState doğrulama kontrolü
	if (!ModelState.IsValid)
	{
		ViewBag.Error = "Please fill in all required fields correctly.";
		return View();
	}

	// Boş değer kontrolü
	if (string.IsNullOrEmpty(userToLogin.Username) || string.IsNullOrEmpty(userToLogin.Password))
	{
		ViewBag.Error = "Username and password cannot be empty.";
		return View();
	}

	// Kullanıcıyı doğrula
	var user = await _userManager.FindByNameAsync(userToLogin.Username);
	if (user == null)
	{
		ModelState.AddModelError("", "User does not exist.");
	}

	var result = await _signInManager.PasswordSignInAsync(userToLogin.Username, userToLogin.Password, false, lockoutOnFailure: false);

	if (result.Succeeded)
	{
		// Giriş başarılıysa anasayfaya yönlendir
		return RedirectToAction("Index", "Home");
	}
	else
	{
		// Giriş başarısızsa hata mesajı göster
		ViewBag.Error = "Invalid username or password.";
		return View();
	}
}

	public IActionResult Register(){
		return View();
	}
[HttpPost]
public async Task<IActionResult> Register(RegisterUserView userToRegister)
{
    if (!ModelState.IsValid)
    {
        return View();
    }

    // Kullanıcı adı veya e-posta var mı kontrol et
    var existingUser = await _userManager.FindByNameAsync(userToRegister.Username);
    var existingEmail = await _userManager.FindByEmailAsync(userToRegister.Email);

    if (existingUser != null)
    {
        ModelState.AddModelError("Username", "This username is already taken.");
        return View();
    }

    if (existingEmail != null)
    {
        ModelState.AddModelError("Email", "This email is already registered.");
        return View();
    }

    // Identity Kullanıcısını Oluştur
    var user = new IdentityUser
    {
        UserName = userToRegister.Username,
        Email = userToRegister.Email
    };

    var result = await _userManager.CreateAsync(user, userToRegister.Password);

    if (result.Succeeded)
    {
        Role? userRole = _context.Roles.FirstOrDefault(r => r.RoleName == "User");

        // Kullanıcı başarılı oluşturulursa Movie Tablosuna Kayıt Ekle
        var newUser = new User
        {
            Name = userToRegister.Name,
            Surname = userToRegister.Surname,
            Username = userToRegister.Username,
            Password = userToRegister.Password,
            Email = userToRegister.Email,
            Role = userRole!
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // Kullanıcıyı otomatik giriş yaptır
        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToAction("Login", "Account");
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError(string.Empty, error.Description);
    }

    return View();
}

		[HttpGet]
		public async Task<IActionResult> Logout(){
		await _signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
	[HttpGet]
	public IActionResult UserDetail(){
		var loginUser = _context.Users.FirstOrDefault(u=>u.Username == User.Identity.Name);

		var viewLoginUser = new UserView{
			UserId = loginUser.UserId,
			Name = loginUser.Name,
			Surname = loginUser.Surname,
			UserName = loginUser.Username,
			Email = loginUser.Email
		};
		return View(viewLoginUser);
	}
	public IActionResult UserUpdate(int userId)
{
	// userId ile eşleşen kullanıcıyı bul
	var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

	// Eğer kullanıcı bulunamazsa, uygun bir hata mesajı döndür
	if (user == null)
	{
		return NotFound("User not found.");
	}

	// Kullanıcıyı güncelleme modeline aktar
	var updateUser = new UserUpdateView
	{
		UserId = userId,
		Name = user.Name ?? string.Empty,
		Surname = user.Surname ?? string.Empty,
		Username = user.Username ?? string.Empty,
		Email = user.Email ?? string.Empty
	};

	// View'e model ile birlikte dön
	return View(updateUser);
}

   [HttpPost]
public IActionResult UserUpdate(UserUpdateView updatedUser)
{
	if (!ModelState.IsValid)
	{
		// Model geçerli değilse, formu tekrar göster ve hataları ilet
		return View(updatedUser);
	}

	var user = _context.Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);

	if (user == null)
	{
		return NotFound("User not found.");
	}

	// Kullanıcı bilgilerini güncelle
	user.Name = updatedUser.Name;
	user.Surname = updatedUser.Surname;
	user.Username = updatedUser.Username;
	user.Email = updatedUser.Email;
	user.Password = updatedUser.Password;

	// Değişiklikleri kaydet
	_context.SaveChanges();

	// Başarı mesajı ekle
	TempData["SuccessMessage"] = "User information updated successfully.";
	return RedirectToAction("UserDetail","Account");
}

public async Task<IActionResult> Delete(int userId)
		{
			var willRemoveUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

			if (willRemoveUser == null)
			{
				return NotFound(); 
			}

			_context.Users.Remove(willRemoveUser);
			await _context.SaveChangesAsync();

			var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == willRemoveUser.Username);
			if (identityUser != null)
			{
				var deleteResult = await _userManager.DeleteAsync(identityUser);
				if (!deleteResult.Succeeded)
				{
					return BadRequest("Kullanıcı Identity'den silinemedi"); 
				}

				if(userId == willRemoveUser.UserId)
				{
					await _signInManager.SignOutAsync(); 
				}
			}

			return RedirectToAction("AllUsers","Manage"); 
		}



}