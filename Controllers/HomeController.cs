using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Movie_Project.Models;

namespace Movie_Project.Controllers;

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


   
}
