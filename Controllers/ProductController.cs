using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Services;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers;

[Authorize(Roles = "admin")]
public class ProductController : Controller
{
    private readonly IProductRepository _products;
    private readonly IWebHostEnvironment _environment;

    public ProductController(IProductRepository products, IWebHostEnvironment environment)
    {
        _products = products;
        _environment = environment;
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

    [HttpGet]
    public IActionResult Create() => View(new ProductFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel viewModel)
    {
        if (!ValidateImage(viewModel.Image))
        {
            return View(viewModel);
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var product = ToProduct(viewModel);
        product.ImageName = await SaveImageAsync(viewModel.Image);
        _products.Create(product);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _products.GetById(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(ToFormViewModel(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductFormViewModel viewModel)
    {
        var existing = _products.GetById(viewModel.Id);
        if (existing == null)
        {
            return NotFound();
        }

        if (!ValidateImage(viewModel.Image))
        {
            viewModel.CurrentImageName = existing.ImageName;
            return View(viewModel);
        }

        if (!ModelState.IsValid)
        {
            viewModel.CurrentImageName = existing.ImageName;
            return View(viewModel);
        }

        var product = ToProduct(viewModel);
        product.ImageName = existing.ImageName;

        if (viewModel.Image != null)
        {
            DeleteImage(existing.ImageName);
            product.ImageName = await SaveImageAsync(viewModel.Image);
        }

        _products.Update(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var product = _products.GetById(id);
        if (product != null)
        {
            DeleteImage(product.ImageName);
            _products.Delete(id);
        }

        return RedirectToAction(nameof(Index));
    }

    private Product ToProduct(ProductFormViewModel viewModel) => new()
    {
        Id = viewModel.Id,
        Name = viewModel.Name,
        Description = viewModel.Description,
        Price = viewModel.Price,
        Rating = viewModel.Rating,
        Amount = viewModel.Amount,
        Category = viewModel.Category
    };

    private ProductFormViewModel ToFormViewModel(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Rating = product.Rating,
        Amount = product.Amount,
        Category = product.Category,
        CurrentImageName = product.ImageName
    };

    private bool ValidateImage(IFormFile? image)
    {
        if (image == null)
        {
            return true;
        }

        if (!image.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(ProductFormViewModel.Image), "Файл повинен бути зображенням");
            return false;
        }

        if (image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError(nameof(ProductFormViewModel.Image), "Зображення не повинно бути більшим за 2 МБ");
            return false;
        }

        return true;
    }

    private async Task<string?> SaveImageAsync(IFormFile? image)
    {
        if (image == null)
        {
            return null;
        }

        var extension = Path.GetExtension(image.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var directory = Path.Combine(_environment.WebRootPath, "images", "products");
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);

        await using var stream = System.IO.File.Create(path);
        await image.CopyToAsync(stream);

        return fileName;
    }

    private void DeleteImage(string? imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
        {
            return;
        }

        var path = Path.Combine(_environment.WebRootPath, "images", "products", imageName);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}
