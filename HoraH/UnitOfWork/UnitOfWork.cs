using HoraH.Domain.Interfaces.Accessor;
using HoraH.Domain.Interfaces.UnitOfWork;
using HoraH.Domain.Models;

namespace HoraH.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbClientAccessor _dbClientAccessor;
    private readonly IDbSessionAccessor _dbSessionAccessor;
    public UnitOfWork(IDbClientAccessor dbClientAccessor, IDbSessionAccessor dbSessionAccessor)
    {
        _dbClientAccessor = dbClientAccessor;
        _dbSessionAccessor = dbSessionAccessor;
    }

    public async Task StartTransactionAsync()
    {
        _dbClientAccessor.ConnectIfNull();
        var client = _dbClientAccessor.DbClient;
        var session = await client.StartSessionAsync();
        session.StartTransaction();
        _dbSessionAccessor.DbSession = session;
    }

    public async Task CommitTransactionAsync()
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            throw new ArgumentNullException(Message.ErroTransacaoNaoIniciadaCommitRollback);
        }
        if (!session.IsInTransaction)
        {
            throw new ArgumentNullException(Message.ErroNaoEhTransacao);
        }
        await session.CommitTransactionAsync();
        _dbSessionAccessor.DbSession = null;
    }

    public async Task RollbackTransactionAsync()
    {
        var session = _dbSessionAccessor.DbSession;
        if (session == null)
        {
            throw new ArgumentNullException(Message.ErroTransacaoNaoIniciadaCommitRollback);
        }
        if (!session.IsInTransaction)
        {
            throw new ArgumentNullException(Message.ErroNaoEhTransacao);
        }
        await session.AbortTransactionAsync();
        _dbSessionAccessor.DbSession = null;
    }

    public async Task<bool> ExecuteTransactionAndReturnOkAsync(Func<Task> asyncTransaction)
    {
        await StartTransactionAsync();
        try
        {
            await asyncTransaction();
            await CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await RollbackTransactionAsync();
            return false;
        }
    }
}