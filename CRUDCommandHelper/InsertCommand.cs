using AutoMapper;
using CLIHelper;
using EFCoreHelper;

namespace CRUDCommandHelper;

public abstract class InsertCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IInsertCommand<TArgumentModel>
        where TEntity : new()
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly IOutput output;
    private readonly IMapper mapper;

    public InsertCommand(
        TUnitOfWork unitOfWork
        , IOutput output
        , IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        this.output = output;
        this.mapper = mapper;

        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.output);
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
        catch (ArgumentException ex)
        {
            output.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            output.WriteLine(ex.Message);
        }
    }

    protected abstract void InsertEntity(TEntity entity);
}