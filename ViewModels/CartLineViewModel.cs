using OnlineShop.Models;

namespace OnlineShop.ViewModels;

public class CartLineViewModel
{
    public required Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => Product.Price * Quantity;
}
