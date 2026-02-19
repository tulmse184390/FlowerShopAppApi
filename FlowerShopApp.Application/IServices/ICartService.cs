using FlowerShopApp.Application.DTOs.Cart;

namespace FlowerShopApp.Application.IServices
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(int userId, AddToCartDto request);
        Task<CartDto> GetCartByUserIdAsync(int userId);
        Task<bool> UpdateQuantityAsync(int userId, UpdateCartDto request);
        Task<bool> RemoveItemAsync(int userId, int productId);
        Task<bool> ClearCartAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
    }
}
