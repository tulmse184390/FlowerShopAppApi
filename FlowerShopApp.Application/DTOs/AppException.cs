namespace FlowerShopApp.Application.DTOs
{
    public class AppException : Exception
    {
        public AppException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
