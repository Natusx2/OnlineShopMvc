namespace OnlineShop.ViewModels;

public class CartViewModel
{
    public IReadOnlyCollection<CartLineViewModel> Lines { get; set; } = [];
    public decimal Total => Lines.Sum(line => line.LineTotal);
    public int TotalQuantity => Lines.Sum(line => line.Quantity);
}
