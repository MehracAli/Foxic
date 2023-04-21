using FOXIC.Entities.UserModels;
using FOXIC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FOXIC.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_roleManager = roleManager;
        }

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterVM registeredUser)
		{
			if (!ModelState.IsValid) return View();
			if (!registeredUser.Terms) return View();
			User user = new()
			{
				Name = registeredUser.Name,
				Surname = registeredUser.Surname,
				Email = registeredUser.Email,
				UserName = registeredUser.UserName,
			};
			IdentityResult result = await _userManager.CreateAsync(user, registeredUser.Password);
			if (!result.Succeeded)
			{
				foreach (IdentityError item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
				return View();
			}
			await _userManager.AddToRoleAsync(user, "Member");
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLoginVM loggedUser)
		{
			if (!ModelState.IsValid) return View();

			User user = await _userManager.FindByNameAsync(loggedUser.UserName);

			if (user is null)
			{
				ModelState.AddModelError("", "Incorrect Username or Password");
				return View();
			}
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
					.PasswordSignInAsync(user, loggedUser.Password, loggedUser.RememberMe, true);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					ModelState.AddModelError("", "You can try to login after 5 minutes");
					return View();
				}
				ModelState.AddModelError("", "Incorrect Username or Password");
				return View();
			}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Logout()
		{
			_signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Details()
		{
			User user = await _userManager.FindByNameAsync(User.Identity.Name);

			return View(user);
		}

		public IActionResult Wishlist()
		{
			return View();
		}

		public IActionResult History()
		{
			return View();
		}
		//public async Task AddRoles()
		//{
		//	await _roleManager.CreateAsync(new IdentityRole("Admin"));
		//	await _roleManager.CreateAsync(new IdentityRole("Member"));
		//}
	}
}
