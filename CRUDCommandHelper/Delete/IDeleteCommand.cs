using ModelHelper;

namespace CRUDCommandHelper;

public interface IDeleteCommand<TArgumentModel>
    where TArgumentModel : IId
{
    void Delete(TArgumentModel model);
}