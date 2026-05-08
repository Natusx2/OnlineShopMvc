using OnlineShop.Models;

namespace OnlineShop.Services;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products =
    [
        new()
        {
            Id = 1,
            Name = "Ноутбук Acer Swift 14",
            Description = "Легкий ноутбук для навчання, роботи та щоденних задач. Має швидкий SSD, якісний IPS-екран і зручну клавіатуру.",
            Price = 34999,
            Rating = 4.8,
            Amount = 8,
            Category = "Ноутбуки"
        },
        new()
        {
            Id = 2,
            Name = "Смартфон Samsung Galaxy A56",
            Description = "Сучасний смартфон з яскравим AMOLED-дисплеєм, гарною камерою та батареєю на цілий день.",
            Price = 18999,
            Rating = 4.6,
            Amount = 14,
            Category = "Смартфони"
        },
        new()
        {
            Id = 3,
            Name = "Навушники Sony WH-1000XM5",
            Description = "Бездротові навушники з активним шумозаглушенням, чистим звуком і м'якою посадкою.",
            Price = 12999,
            Rating = 4.9,
            Amount = 0,
            Category = "Аудіо"
        },
        new()
        {
            Id = 4,
            Name = "Монітор LG UltraGear 27",
            Description = "Ігровий 27-дюймовий монітор з високою частотою оновлення, чіткою картинкою та тонкими рамками.",
            Price = 9999,
            Rating = 4.7,
            Amount = 5,
            Category = "Монітори"
        },
        new()
        {
            Id = 5,
            Name = "Клавіатура Logitech MX Keys",
            Description = "Тиха бездротова клавіатура з підсвіткою, металевою основою та швидким перемиканням між пристроями.",
            Price = 4999,
            Rating = 4.5,
            Amount = 21,
            Category = "Периферія"
        },
        new()
        {
            Id = 6,
            Name = "Миша Razer Basilisk V3",
            Description = "Ергономічна ігрова миша з точним сенсором, програмованими кнопками та плавним колесом прокрутки.",
            Price = 3299,
            Rating = 4.4,
            Amount = 0,
            Category = "Периферія"
        }
    ];

    private int _nextId = 7;

    public IReadOnlyCollection<Product> GetAll(string? sort = null)
    {
        IEnumerable<Product> query = _products;

        query = sort switch
        {
            ProductSortOptions.PriceDesc => query.OrderByDescending(p => p.Price),
            ProductSortOptions.Name => query.OrderBy(p => p.Name),
            ProductSortOptions.RatingDesc => query.OrderByDescending(p => p.Rating),
            _ => query.OrderBy(p => p.Price)
        };

        return query.ToList();
    }

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public Product Create(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return product;
    }

    public bool Update(Product product)
    {
        var existing = GetById(product.Id);
        if (existing == null)
        {
            return false;
        }

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Rating = product.Rating;
        existing.Amount = product.Amount;
        existing.Category = product.Category;
        existing.ImageName = product.ImageName;
        return true;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        return product != null && _products.Remove(product);
    }
}
