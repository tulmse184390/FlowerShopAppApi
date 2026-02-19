namespace FlowerShopApp.Domain.Interfaces
{
    public interface IGlobalTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
