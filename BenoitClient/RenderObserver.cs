using System;
using Orleans;
using BenoitGrainInterfaces;
using BenoitCommons;
using Orleans.Concurrency;

namespace BenoitClient
{
    public class RenderObserver : IRenderObserver<int>
    {
        public event ReceivedRenderedFrameHandler OnReceivedRenderedFrame;
        public event ReceivedRenderedMovieHandler OnReceivedRenderedMovie;
        
        public delegate void ReceivedRenderedFrameHandler(Guid requestIdentifier, Immutable<Map2D<int>> frame);
        public delegate void ReceivedRenderedMovieHandler(Guid requestIdentifier, Immutable<Map2D<int>[]> movie);

        public void ReceiveRenderedFrame(Guid requestIdentifier, Immutable<Map2D<int>> frame)
        {
            OnReceivedRenderedFrame?.Invoke(requestIdentifier, frame);
        }

        public void ReceiveRenderedMovie(Guid requestIdentifier, Immutable<Map2D<int>[]> movie)
        {
            OnReceivedRenderedMovie?.Invoke(requestIdentifier, movie);
        }
    }
}