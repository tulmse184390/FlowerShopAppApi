using AutoMapper;
using FlowerShopApp.Application.DTOs.Categories;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.Entities.ToListAsync();

            return _mapper.Map<List<CategoryDto>>(categories);  
        }
    }
}
