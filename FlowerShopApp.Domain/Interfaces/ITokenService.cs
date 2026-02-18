using FlowerShopApp.Domain.Entities;

namespace FlowerShopApp.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
}
