namespace CRUDCommandHelper;

public interface IInsertManyToManyCommand
{
    void Insert(int modelId1, int modelId2);
}