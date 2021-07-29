namespace Localizati18n.WebApp.Controllers {
  using System;
  using System.Diagnostics;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Localizati18n.WebApp.Models;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Localization;

  public class HomeController : Controller {
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger) {
      this.logger = logger;
    }

    public IActionResult Index() {
      return this.View();
    }

    public IActionResult Privacy() {
      return this.View();
    }
    
    [HttpPost]
    public IActionResult SetLanguage(string language) {
      this.Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
      );

      return this.LocalRedirect("/" + this.ControllerContext.ActionDescriptor.ControllerName);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
  }
}