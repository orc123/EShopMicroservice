namespace Basket.API.Data;

public class CacheBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string UserName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(UserName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }
        var basket = await repository.GetBasket(UserName, cancellationToken);
        await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket));

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

        return basket;
    }

    public async Task<bool> DeleteBasket(string UserName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(UserName, cancellationToken);

        await cache.RemoveAsync(UserName, cancellationToken);

        return true;
    }
}
