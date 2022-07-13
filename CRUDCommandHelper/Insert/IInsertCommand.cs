namespace CRUDCommandHelper;

public interface IInsertCommand<TArgumentModel>
{
    void Insert(TArgumentModel argumentModel);
}