using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;

namespace BenoitGrainInterfaces
{
    // http://dotnet.github.io/orleans/Documentation/grains/observers.html
    public interface IRenderObserver<TExport> : IGrainObserver
        where TExport : IConvertible
    {
        void ReceiveRenderedFrame(Immutable<Map2D<TExport>> frame);
        void ReceiveRenderedMovie(Immutable<Map2D<TExport>[]> movie);
    }
}