using OnlineShop.Models;

namespace OnlineShop.Services;

public interface IProductRepository
{
    IReadOnlyCollection<Product> GetAll(string? sort = null);
    Product? GetById(int id);
    Product Create(Product product);
    bool Update(Product product);
    bool Delete(int id);
}
