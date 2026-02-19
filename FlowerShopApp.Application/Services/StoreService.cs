using AutoMapper;
using FlowerShopApp.Application.DTOs.Store;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Interfaces;

namespace FlowerShopApp.Application.Services
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<StoreLocationDto>> GetAllStoresAsync()
        {
            var stores = await _unitOfWork.StoreLocations.GetAllAsync();
            return _mapper.Map<List<StoreLocationDto>>(stores);
        }
    }
}