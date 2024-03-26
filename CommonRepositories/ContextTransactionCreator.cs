using Common.Application.Abstractions.Persistence;
using Common.Persistence.Context;

namespace Common.Persistence
{
    public class ContextTransactionCreator:IContextTransactionCreator
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ContextTransactionCreator(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return new ContextTransaction(await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken));
        }
    }
}
