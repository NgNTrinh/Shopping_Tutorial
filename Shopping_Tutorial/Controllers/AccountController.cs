using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Models.ViewModels;
using System.Security.Claims;

namespace Shopping_Tutorial.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var account = await _userManager.FindByIdAsync(userId);
			return View(account);
		}

		public IActionResult Create()
		{
			return View();
		}
		public async Task<IActionResult> Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email };
				IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

				if (result.Succeeded)
				{
					TempData["success"] = "Tạo tài khoản thành công";
					return Redirect("/account/login");
				}

				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);

				if (result.Succeeded)
				{
					return Redirect(loginVM.ReturnUrl ?? "/");
				}

				ModelState.AddModelError("", "Username or Password sai ?");
			}

			return View(loginVM);
		}

		public async Task<RedirectResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();

			return Redirect(returnUrl);
		}
	}
}

