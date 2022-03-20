using CLIHelper;
using DataToTable;
using EFCoreHelper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class ReadCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IReadCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly IOutput output;
    protected readonly ILogger Log;
    private readonly IDataToText<TEntity> textProvider;

    public ReadCommand(
        TUnitOfWork unitOfWork
        , IOutput output
        , ILogger log
        , IDataToText<TEntity> textProvider)
    {
        UnitOfWork = unitOfWork;
        this.output = output;
        this.Log = log;
        this.textProvider = textProvider;

        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.output);
        ArgumentNullException.ThrowIfNull(this.Log);
        ArgumentNullException.ThrowIfNull(this.textProvider);
    }

    public void Read(TArgumentModel model)
    {
        output.Clear();
        Log.Information(
            "{0} {1}", nameof(Read), typeof(TEntity).Name);
        output.Write(
            textProvider.GetText(
                Get(model)));
    }

    protected abstract List<TEntity> Get(TArgumentModel model);
}