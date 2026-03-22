using HappyPrograming.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly HappyprogrammingContext _context;

    public AccountController(HappyprogrammingContext context)
    {
        _context = context;
    }

    // ======================
    // GET: LOGIN
    // ======================
    public IActionResult Login()
    {
        return View();
    }

    // ======================
    // POST: LOGIN
    // ======================
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        // ✅ FIX ROLE
        string roleName = "";

        if (user.RoleId == 1)
            roleName = "Admin";
        else if (user.RoleId == 2)
            roleName = "Mentor";
        else if (user.RoleId == 3)
            roleName = "Mentee";

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, roleName)
    };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal);

        return RedirectToAction("Dashboard", "Admin");
    }

    // ======================
    // LOGOUT
    // ======================
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}