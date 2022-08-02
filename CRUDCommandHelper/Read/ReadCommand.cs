using CLIHelper;
using DataToTable;
using EFCore.Helper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class ReadCommand<TUnitOfWork, TEntity, TArgumentModel>
    : IReadCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork;
    protected readonly IOutput Output;
    protected readonly ILogger Log;
    private readonly IDataToText<TEntity> textProvider;

    public ReadCommand(
        TUnitOfWork unitOfWork
        , IOutput output
        , ILogger log
        , IDataToText<TEntity> textProvider)
    {
        UnitOfWork = unitOfWork;
        this.Output = output;
        this.Log = log;
        this.textProvider = textProvider;
        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.Output);
        ArgumentNullException.ThrowIfNull(this.Log);
        ArgumentNullException.ThrowIfNull(this.textProvider);
    }

    public void Read(TArgumentModel model)
    {
        Output.Clear();
        Log.Information(
            "{0} {1}", nameof(Read), typeof(TEntity).Name);
        Output.Write(
            textProvider.GetText(
                Get(model)));
    }

    protected abstract List<TEntity> Get(TArgumentModel model);
}