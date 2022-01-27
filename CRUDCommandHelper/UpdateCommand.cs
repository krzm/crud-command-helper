using AutoMapper;
using CLIHelper;
using EFCoreHelper;
using ModelHelper;

namespace CRUDCommandHelper;

public abstract class UpdateCommand<TUnitOfWork, TEntity, TArgumentModel, TUpdateModel>
    : IUpdateCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
        where TUpdateModel : IUpdatable<TEntity>
        where TArgumentModel : IId
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly IOutput output;
    private readonly IMapper mapper;

    public UpdateCommand(
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

    public virtual void Update(TArgumentModel model)
    {
        try
        {
            var updateModel = mapper.Map<TUpdateModel>(model);
            var modelDb = GetById(model.Id);
            if (modelDb == null) throw new Exception($"No Id:{model.Id} in database.");
            updateModel.Update(modelDb);
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

    protected abstract TEntity GetById(int id);
}