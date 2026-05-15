using System.Text.Json;
using OnlineShop.Models;

namespace OnlineShop.Services;

public static class CartSessionService
{
    private const string CartKey = "cart";

    public static List<CartItem> GetItems(ISession session)
    {
        var json = session.GetString(CartKey);
        return string.IsNullOrWhiteSpace(json)
            ? []
            : JsonSerializer.Deserialize<List<CartItem>>(json) ?? [];
    }

    public static int GetCount(ISession session) => GetItems(session).Sum(item => item.Quantity);

    public static void Add(ISession session, int productId, int maxQuantity)
    {
        if (maxQuantity < 1)
        {
            return;
        }

        var items = GetItems(session);
        var item = items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
        {
            items.Add(new CartItem { ProductId = productId, Quantity = 1 });
        }
        else if (item.Quantity < maxQuantity)
        {
            item.Quantity++;
        }

        SaveItems(session, items);
    }

    public static void Decrease(ISession session, int productId)
    {
        var items = GetItems(session);
        var item = items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
        {
            return;
        }

        item.Quantity--;
        if (item.Quantity < 1)
        {
            items.Remove(item);
        }

        SaveItems(session, items);
    }

    public static void Remove(ISession session, int productId)
    {
        var items = GetItems(session);
        items.RemoveAll(i => i.ProductId == productId);
        SaveItems(session, items);
    }

    private static void SaveItems(ISession session, List<CartItem> items)
    {
        session.SetString(CartKey, JsonSerializer.Serialize(items));
    }
}
