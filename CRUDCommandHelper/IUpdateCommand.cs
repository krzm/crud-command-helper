using ModelHelper;

namespace CRUDCommandHelper;

public interface IUpdateCommand<TArgumentModel>
    where TArgumentModel : IId
{
    void Update(TArgumentModel model);
}