using CLIHelper;
using DataToTable;
using EFCoreHelper;

namespace CRUDCommandHelper;

public abstract class ReadCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IReadCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly IOutput output;
    private readonly IDataToText<TEntity> textProvider;

    public ReadCommand(
        TUnitOfWork unitOfWork
        , IOutput output
        , IDataToText<TEntity> textProvider)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(textProvider);

        this.UnitOfWork = unitOfWork;
        this.output = output;
        this.textProvider = textProvider;
    }

    public void Read(TArgumentModel model)
    {
        var data = Get(model);
        var text = textProvider.GetText(data);
        output.Clear();
        output.WriteLine($"{nameof(Read)} {typeof(TEntity).Name}:");
        output.Write(text);
    }

    protected abstract List<TEntity> Get(TArgumentModel model);
}