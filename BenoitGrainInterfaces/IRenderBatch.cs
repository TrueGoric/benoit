using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    public interface IRenderBatch<TExport> : IGrainWithGuidKey
        where TExport : IConvertible
    {
        Task<Immutable<TExport[]>> Compute(RenderingOptions options, Complex[] coordinates);
    }
}