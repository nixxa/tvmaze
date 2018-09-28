using LiteDB;

namespace Kernel.Interfaces
{
    public interface IDataProviderFactory
    {
        LiteDatabase Create();
    }
}