namespace Kernel.Interfaces
{
    public interface IPagingParameters
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}