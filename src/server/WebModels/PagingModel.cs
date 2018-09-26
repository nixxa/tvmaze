using Core.Interfaces;

namespace WebModels
{
    public class PagingModel : IPagingParameters
    {
        public const int DefaultPage = 1;
        public const int DefaultPageSize = 1;

        public PagingModel()
        {
            Page = DefaultPage;
            PageSize = DefaultPageSize; 
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}