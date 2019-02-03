using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IMovieRenderer<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task<I2DMap<TExport>[]> Render(RenderingOptions options, Complex center, double scale, double scaleMultiplier, int frames);
    }
}