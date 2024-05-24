using Core.Models;
using ExamApp.DTO_s;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExamApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager = null, SignInManager<User> signInManager = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

 
        public IActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return View();

            var exitingName = await _userManager.FindByNameAsync(registerDto.Username);
            var exitingEmail = await _userManager.FindByEmailAsync(registerDto.Email);

            if (exitingName != null)
            {
                ModelState.AddModelError("", "Bu username ile hesab artiq movcuddur!");
                return View();
            }
            if (exitingEmail != null)
            {
                ModelState.AddModelError("", "Bu email ile hesab artiq movcuddur!");
                return View();
            }
            User appUser = new User()
            {
                Name = registerDto.FirstName,
                Surname = registerDto.Lastname,
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(appUser,registerDto.Password);

            if (!result.Succeeded)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            await _signInManager.SignInAsync(appUser, false);
            await _userManager.AddToRoleAsync(appUser, "Member");


            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logout");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logout");
            }
            if (!ModelState.IsValid) return View();
            User appUser;

            if(loginDto.UsernameOrEmail.Contains("@"))
            {
                appUser= await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
            }
            else
            {
                appUser = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            }

            if(appUser == null)
            {
                ModelState.AddModelError("", "Username or email ve ya password sehvdir");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Birazdan yeniden cehd edin!");
                return View();
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or email ve ya password sehvdir");
                return View();
            }

            await _signInManager.SignInAsync(appUser, loginDto.RememberMe);

            var role = await _userManager.GetRolesAsync(appUser);

            if (role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("Admin");
            IdentityRole role2 = new IdentityRole("Moderator");
            IdentityRole role3 = new IdentityRole("Member");
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            return Ok();
        }
    }
}
