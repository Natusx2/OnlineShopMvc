using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Вкажіть назву товару")]
    [StringLength(120, ErrorMessage = "Назва не може бути довшою за 120 символів")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Додайте опис товару")]
    [StringLength(1000, ErrorMessage = "Опис не може бути довшим за 1000 символів")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "Ціна повинна бути більшою за 0")]
    public decimal Price { get; set; }

    [Range(0, 5, ErrorMessage = "Рейтинг повинен бути від 0 до 5")]
    public double Rating { get; set; }

    [Range(0, 100_000, ErrorMessage = "Кількість не може бути від'ємною")]
    public int Amount { get; set; }

    public string? ImageName { get; set; }
    public string Category { get; set; } = "Товари";
}
