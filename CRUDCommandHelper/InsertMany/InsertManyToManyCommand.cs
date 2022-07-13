using AutoMapper;
using EFCore.Helper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class InsertManyToManyCommand<TUnitOfWork>
    : IInsertManyToManyCommand
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly ILogger log;
    private readonly IMapper mapper;

    public InsertManyToManyCommand(
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

    public virtual void Insert(
        int modelId1
        , int modelId2)
    {
        try
        {
            InsertEntity(modelId1, modelId2);
            UnitOfWork.Save();
        }
        catch (Exception ex)
        {
            log.Error(ex, "Insert Error");
        }
    }

    protected abstract void InsertEntity(int modelId1, int modelId2);
}