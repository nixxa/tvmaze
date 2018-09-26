using LiteDB;

namespace Core.Interfaces
{
    public interface IDataProviderFactory
    {
        LiteDatabase Create();
    }
}