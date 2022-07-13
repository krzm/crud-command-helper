namespace CRUDCommandHelper;

public abstract class Command
{
    protected const string NotInDbError = "No Id: {0} in database";
    protected const string DeleteError = "Error during delete on Id: {0}";
    protected const string DeleteYesNo = "Are you sure you want to delete id: {0} ?";
    protected const string Yes = "y";
    protected const string No = "n";
    protected const string InputYesNo = "Input y or n";
}