using CLIHelper;
using EFCore.Helper;
using ModelHelper;
using Serilog;

namespace CRUDCommandHelper;

public abstract class DeleteCommand<TUnitOfWork, TEntity, TArgumentModel>
    : Command
    , IDeleteCommand<TArgumentModel>
        where TUnitOfWork : IUnitOfWork
        where TArgumentModel : IId
{
    protected readonly TUnitOfWork UnitOfWork;
    private readonly ILogger log;
    private readonly IInput input;

    public DeleteCommand(
        TUnitOfWork unitOfWork
        , ILogger log
        , IInput input)
    {
        UnitOfWork = unitOfWork;
        this.log = log;
        this.input = input;
        ArgumentNullException.ThrowIfNull(UnitOfWork);
        ArgumentNullException.ThrowIfNull(this.log);
        ArgumentNullException.ThrowIfNull(this.input);
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
        if(AskAreYouSure(model) == false)
            return;
        var modelDb = GetById(model.Id);
        ArgumentNullException.ThrowIfNull(modelDb);
        DeleteEntity(modelDb);
        UnitOfWork.Save();
    }

    private bool AskAreYouSure(TArgumentModel model)
    {
        log.Information(DeleteYesNo, model.Id);
        var line = input.ReadLine();
        while (IsYesOrNo(line) == false)
        {
            log.Information(InputYesNo);
            line = input.ReadLine();
        }
        if (line == Yes)
            return true;
        return false;
    }

    private static bool IsYesOrNo(string? line)
    {
        var yes = string.Equals(line, Yes) == true;
        var no = string.Equals(line, No) == true;
        var yesOrNo = yes || no;
        return yesOrNo;
    }

    protected abstract TEntity GetById(int id);

    protected abstract void DeleteEntity(TEntity entity);
}