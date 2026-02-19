using FlowerShopApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace FlowerShopApp.Infrastructure.Implements
{
    public class EfGlobalTransaction : IGlobalTransaction
    {
        private readonly IDbContextTransaction _target;

        public EfGlobalTransaction(IDbContextTransaction target)
        {
            _target = target;
        }

        public void Commit()
        {
            _target.Commit();
        }

        public void Dispose()
        {
            _target.Dispose();
        }

        public void Rollback()
        {
            _target.Rollback();
        }
    }
}
