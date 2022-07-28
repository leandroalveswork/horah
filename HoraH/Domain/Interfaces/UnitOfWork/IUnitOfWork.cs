namespace HoraH.Domain.Interfaces.UnitOfWork;
public interface IUnitOfWork
{
    Task StartTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> ExecuteTransactionAndReturnOkAsync(Func<Task> asyncTransaction);
}