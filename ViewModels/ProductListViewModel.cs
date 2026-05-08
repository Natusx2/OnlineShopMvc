using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.ViewModels;

public class ProductListViewModel
{
    public IReadOnlyCollection<Product> Products { get; set; } = [];
    public string Sort { get; set; } = ProductSortOptions.PriceAsc;
}
