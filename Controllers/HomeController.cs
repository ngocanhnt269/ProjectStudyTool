using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectStudyTool.Models;

namespace ProjectStudyTool.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Get user's study contents on Home page
    [HttpPost]
    public IActionResult Index(string userContent)
    {
        if (!ValidateContent(userContent))
        {
            Console.WriteLine("Empty user content");
            return View();
        }
        Console.WriteLine(userContent);
        return View();
    }

    // Validate user's study contents
    public bool ValidateContent(string userContent)
    {
        if (string.IsNullOrEmpty(userContent))
        {
            return false;
        }
        return true;
    }
}
