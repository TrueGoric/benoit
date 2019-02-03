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
        RenderingOptions Options { get; set; }
        Task<I2DMap<TExport>> RenderFrame(Complex center, double scale);
        Task<I2DMap<TExport>[]> RenderMovie(Complex center, double scale, double scaleMultiplier, int frames);
    }
}
