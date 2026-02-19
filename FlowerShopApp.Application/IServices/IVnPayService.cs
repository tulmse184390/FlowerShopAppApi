using FlowerShopApp.Application.DTOs.Payment;
using FlowerShopApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FlowerShopApp.Application.IServices
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, Order order); 
        PaymentResponseDto PaymentExecute(IQueryCollection collections);
    }
}
