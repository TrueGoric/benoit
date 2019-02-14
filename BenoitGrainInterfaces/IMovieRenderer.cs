using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IMovieRenderer<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task<Immutable<Map2D<TExport>[]>> Render(RenderingOptions options, Complex center, double scale, double scaleMultiplier, int frames, GrainCancellationToken cancellationToken = null);
    }
}