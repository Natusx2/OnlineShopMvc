using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

public class CartController : Controller
{
    private readonly IProductRepository _products;

    public CartController(IProductRepository products)
    {
        _products = products;
    }

    public IActionResult Index()
    {
        var lines = CartSessionService.GetItems(HttpContext.Session)
            .Select(item => new
            {
                Item = item,
                Product = _products.GetById(item.ProductId)
            })
            .Where(row => row.Product != null)
            .Select(row => new CartLineViewModel
            {
                Product = row.Product!,
                Quantity = Math.Min(row.Item.Quantity, row.Product!.Amount)
            })
            .Where(line => line.Quantity > 0)
            .ToList();

        return View(new CartViewModel { Lines = lines });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int productId, string? returnUrl = null)
    {
        var product = _products.GetById(productId);
        if (product != null)
        {
            CartSessionService.Add(HttpContext.Session, productId, product.Amount);
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Increase(int productId)
    {
        var product = _products.GetById(productId);
        if (product != null)
        {
            CartSessionService.Add(HttpContext.Session, productId, product.Amount);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Decrease(int productId)
    {
        CartSessionService.Decrease(HttpContext.Session, productId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        CartSessionService.Remove(HttpContext.Session, productId);
        return RedirectToAction(nameof(Index));
    }
}
