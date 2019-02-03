using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IRenderBatch<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task<TExport[]> Compute(RenderingOptions options, Complex[] coordinates);
    }
}