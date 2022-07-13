using EFCore.Helper;
using ModelHelper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class DeleteCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IDeleteCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
        where TArgumentModel : IId
{
    private const string NotInDbError = "No Id: {0} in database";
    private const string DeleteError = "Error during delete on Id: {0}";
    protected readonly TUnitOfWork UnitOfWork;
    private readonly ILogger log;

    public DeleteCommand(
        TUnitOfWork unitOfWork
        , ILogger log)
    {
        UnitOfWork = unitOfWork;
        this.log = log;
        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.log);
    }

    public void Delete(TArgumentModel model)
    {
        try
        {
            DeleteTemplate(model);
        }
        catch (ArgumentNullException ex)
        {
            log.Error(ex, NotInDbError, model.Id);
        }
        catch (Exception ex)
        {
            log.Error(ex, DeleteError, model.Id);
        }
    }

    private void DeleteTemplate(TArgumentModel model)
    {
        var modelDb = GetById(model.Id);
        ArgumentNullException.ThrowIfNull(modelDb);
        DeleteEntity(modelDb);
        UnitOfWork.Save();
    }

    protected abstract TEntity GetById(int id);

    protected abstract void DeleteEntity(TEntity entity);
}