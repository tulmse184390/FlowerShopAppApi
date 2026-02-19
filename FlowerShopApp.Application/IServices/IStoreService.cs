using FlowerShopApp.Application.DTOs.Store;

namespace FlowerShopApp.Application.IServices
{
    public interface IStoreService
    {
        Task<List<StoreLocationDto>> GetAllStoresAsync();
    }
}
