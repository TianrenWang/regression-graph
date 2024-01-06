using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pulsenic_app.Models;

namespace pulsenic_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static Curve currentCurve = Curve.LINEAR;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        currentCurve = Curve.LINEAR;
        return View(AppState.GetModel(currentCurve));
    }

    public IActionResult Quadratic()
    {
        currentCurve = Curve.QUADRATIC;
        return View(AppState.GetModel(currentCurve));
    }

    public IActionResult Cubic()
    {
        currentCurve = Curve.CUBIC;
        return View(AppState.GetModel(currentCurve));
    }

    [HttpPost]
    public ActionResult Index(Point point)
    {
        AppState.AddPoint(point);
        return View(AppState.GetModel(currentCurve));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

