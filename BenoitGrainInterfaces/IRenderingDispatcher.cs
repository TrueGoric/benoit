using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IRenderingDispatcher<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task SetOptions(RenderingOptions options);
        Task<RenderingOptions> GetOptions();
        
        [AlwaysInterleave]
        Task<bool> BeginRenderFrame(Guid requestIdentifier, Complex center, double scale);

        [AlwaysInterleave]
        Task<bool> BeginRenderMovie(Guid requestIdentifier, Complex center, double scale, double scaleMultiplier, int frames);
    
        Task Subscribe(IRenderObserver<TExport> observer);
        Task Unsubscribe(IRenderObserver<TExport> observer);
    }
}
