namespace CRUDCommandHelper;

public interface IReadCommand<TArgumentModel>
{
    void Read(TArgumentModel model);
}