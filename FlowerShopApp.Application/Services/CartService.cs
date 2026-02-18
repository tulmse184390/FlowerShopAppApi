using AutoMapper;
using FlowerShopApp.Application.DTOs.Cart;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Entities;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddToCartAsync(int userId, AddToCartDto request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null || product.IsDeleted)
                throw new Exception("The product does not exist!");

            if (product.StockQuantity < request.Quantity)
                throw new Exception($"This product has only {product.StockQuantity} left!");

            var cart = await _unitOfWork.Carts.Entities
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };
                _unitOfWork.Carts.Add(cart);
            }

            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = product.Price;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price 
                };
                cart.CartItems.Add(newItem);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<CartDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.Entities
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)        
                .ThenInclude(p => p.ProductImages)  
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null)
            {
                return new CartDto
                {
                    Items = new List<CartItemDto>(),
                    TotalAmount = 0
                };
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> UpdateQuantityAsync(int userId, UpdateCartDto request)
        {
            var cart = await _unitOfWork.Carts.Entities
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product.StockQuantity < request.Quantity)
            {
                throw new Exception($"Insufficient stock. Only {product.StockQuantity} items left.");
            }

            if (request.Quantity <= 0)
            {
                cart.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = request.Quantity;
                cartItem.Price = product.Price;
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId)
        {
            var cart = await _unitOfWork.Carts.Entities
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null) return false;

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            cart.CartItems.Remove(cartItem);

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.Entities
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Status == "Active");

            if (cart == null) return false;

            cart.CartItems.Clear();

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
