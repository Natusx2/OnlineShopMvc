using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _products;

    public HomeController(ILogger<HomeController> logger, IProductRepository products)
    {
        _logger = logger;
        _products = products;
    }

    public IActionResult Index(string sort = ProductSortOptions.PriceAsc)
    {
        var viewModel = new ProductListViewModel
        {
            Products = _products.GetAll(sort),
            Sort = sort
        };

        return View(viewModel);
    }

    public IActionResult Details(int id)
    {
        var product = _products.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
