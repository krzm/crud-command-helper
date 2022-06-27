using AutoMapper;
using EFCore.Helper;
using ModelHelper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class UpdateCommand<TUnitOfWork, TEntity, TArgumentModel, TUpdateModel>
    : IUpdateCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
        where TUpdateModel : IUpdatable<TEntity>
        where TArgumentModel : IId
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly ILogger log;
    private readonly IMapper mapper;

    public UpdateCommand(
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

    public virtual void Update(TArgumentModel model)
    {
        try
        {
            var updateModel = mapper.Map<TUpdateModel>(model);
            var modelDb = GetById(model.Id);
            ArgumentNullException.ThrowIfNull(modelDb);
            updateModel.Update(modelDb);
            UnitOfWork.Save();
        }
        catch (ArgumentNullException ex)
        {
            log.Error(ex, "No Id: {0} in database", model.Id);
        }
        catch (Exception ex)
        {
            log.Error(ex, "Error during update on Id: {0}", model.Id);
        }
    }

    protected abstract TEntity GetById(int id);
}