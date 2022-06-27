using AutoMapper;
using EFCore.Helper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class InsertCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IInsertCommand<TArgumentModel>
        where TEntity : new()
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly ILogger log;
    private readonly IMapper mapper;

    public InsertCommand(
        TUnitOfWork unitOfWork
        , ILogger log
        , IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        this.log = log;
        this.mapper = mapper;
        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.log);
        ArgumentNullException.ThrowIfNull(this.mapper);
    }

    public virtual void Insert(TArgumentModel argumentModel)
    {
        try
        {
            var model = mapper.Map<TEntity>(argumentModel);
            InsertEntity(model);
            UnitOfWork.Save();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Insert Error");
        }
    }

    protected abstract void InsertEntity(TEntity entity);
}