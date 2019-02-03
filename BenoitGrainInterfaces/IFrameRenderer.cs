using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IFrameRenderer<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task<I2DMap<TExport>> RenderFrame(RenderingOptions options, Complex center, double scale);
    }
}