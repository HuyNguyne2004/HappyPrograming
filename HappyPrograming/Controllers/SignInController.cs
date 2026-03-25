using HappyPrograming.Service;
using Microsoft.AspNetCore.Mvc;

namespace HappyPrograming.Controllers
{
    public class SignInController : Controller
    {
        private readonly AuthService _authService;

        public SignInController(AuthService authService)
        {
            _authService = authService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View("SignInView");
        }


        [HttpPost]
        public IActionResult SubmitLogin(string username, string password)
        {

            var user = _authService.Login(username, password);

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid username or password!";
                return View("SignInView");
            }
        }
    }
}