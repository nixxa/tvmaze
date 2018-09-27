using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Kernel.Actions
{
    public class SynchronizeShowsHandler : IRequestHandler<SynchronizeShowsRequest>
    {
        public Task<Unit> Handle(SynchronizeShowsRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SynchronizeShowsRequest : IRequest
    {
        
    }
}