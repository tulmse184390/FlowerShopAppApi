using AutoMapper;
using FlowerShopApp.Application.DTOs;
using FlowerShopApp.Application.DTOs.Auth;
using FlowerShopApp.Application.IServices;
using FlowerShopApp.Domain.Entities;
using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowerShopApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var isExist = await _unitOfWork.Users.Entities
                .AnyAsync(x => x.UserName.ToLower() == request.UserName.ToLower());

            if (isExist)
            {
                throw new AppException("Username already exists!");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            _unitOfWork.Users.Add(user);
            await _unitOfWork.CompleteAsync();

            var response = _mapper.Map<AuthResponseDto>(user);
            response.Token = _tokenService.GenerateAccessToken(user);

            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _unitOfWork.Users.Entities
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new AppException("Username or password is incorrect!");
            }

            var response = _mapper.Map<AuthResponseDto>(user);
            response.Token = _tokenService.GenerateAccessToken(user);

            return response;
        }
    }
}
