using AlMahaRental.Models;
using AlMahaRental.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlMahaRental.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.IdNumber,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IdNumber = model.IdNumber,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    // === إضافة إشعار: تسجيل عميل جديد ===
                    _context.SystemNotifications.Add(new SystemNotification
                    {
                        Title = "تسجيل عميل جديد",
                        Message = $"انضم العميل {user.FirstName} {user.LastName} إلى عائلة المها حالياً.",
                        Type = "User",
                        LinkUrl = "/Admin/Users",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.IdNumber, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "محاولة دخول غير صحيحة. تأكد من رقم الهوية وكلمة المرور.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

// --- ViewModels (تم وضعها هنا للتبسيط والالتزام بعدد الملفات) ---

public class LoginViewModel
    {
        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "الرقم السري")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "الايميل مطلوب")]
        [Display(Name = "الايميل")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهوية مطلوب")]
        [Display(Name = "رقم الهوية")]
        public string IdNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب أن تكون {0} على الأقل {2} أحرف.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "الرقم السري")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد الرقم السري")]
        [Compare("Password", ErrorMessage = "الرقم السري وتأكيد الرقم السري لا يتطابقان.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}