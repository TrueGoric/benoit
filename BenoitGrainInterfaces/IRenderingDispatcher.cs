using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IRenderingDispatcher<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task SetOptions(RenderingOptions options);
        Task<RenderingOptions> GetOptions();
        
        Task<Map2D<TExport>> RenderFrame(Complex center, double scale);
        Task<Map2D<TExport>[]> RenderMovie(Complex center, double scale, double scaleMultiplier, int frames);
    }
}
