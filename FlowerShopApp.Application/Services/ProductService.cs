using AutoMapper;
using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Products;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.Entities
                .Include(x => x.Category)       
                .Include(x => x.ProductImages)  
                .FirstOrDefaultAsync(x => x.ProductId == id && !x.IsDeleted);

            if (product == null) return null;

            return _mapper.Map<ProductDetailDto>(product);
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(ProductParams paramsDto)
        {
            var query = _unitOfWork.Products.Entities
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(paramsDto.Search))
            {
                query = query.Where(x => x.ProductName.ToLower().Contains(paramsDto.Search.ToLower()));
            }

            if (paramsDto.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == paramsDto.CategoryId);
            }

            if (paramsDto.MinPrice.HasValue) query = query.Where(x => x.Price >= paramsDto.MinPrice);
            if (paramsDto.MaxPrice.HasValue) query = query.Where(x => x.Price <= paramsDto.MaxPrice);

            query = paramsDto.SortBy switch
            {
                "price_asc" => query.OrderBy(x => x.Price),
                "price_desc" => query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.ProductName)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((paramsDto.PageIndex - 1) * paramsDto.PageSize)
                .Take(paramsDto.PageSize)
                .ToListAsync();

            var data = _mapper.Map<IEnumerable<ProductDto>>(products);

            return new PagedResult<ProductDto>
            {
                CurrentPage = paramsDto.PageIndex,
                PageSize = paramsDto.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paramsDto.PageSize),
                Items = data
            };
        }
    }
}