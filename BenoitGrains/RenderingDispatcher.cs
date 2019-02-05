using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using Orleans.Providers;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    [StorageProvider(ProviderName = "OptionsStorage")]
    public class RenderingDispatcher<TExport> : Grain<RenderingOptions>, IRenderingDispatcher<TExport>
        where TExport : IConvertible
    {
        private IFrameRenderer<TExport> _frameRenderer;
        private IMovieRenderer<TExport> _movieRenderer;

        public Task SetOptions(RenderingOptions options)
        {
            State = options;
            return base.WriteStateAsync();
        }

        public Task<RenderingOptions> GetOptions()
        {            
            return Task.FromResult(State);
        }

        public async Task<Immutable<Map2D<TExport>>> RenderFrame(Complex center, double scale)
        {
            if (_frameRenderer == null)
            {
                _frameRenderer = GrainFactory.GetGrain<IFrameRenderer<TExport>>(Guid.NewGuid());
            }

            return await _frameRenderer.RenderFrame(State, center, scale);
        }

        public async Task<Immutable<Map2D<TExport>[]>> RenderMovie(Complex center, double scale, double scaleMultiplier, int frames)
        {
            if (_movieRenderer == null)
            {
                _movieRenderer = GrainFactory.GetGrain<IMovieRenderer<TExport>>(Guid.NewGuid());
            }

            return await _movieRenderer.Render(State, center, scale, scaleMultiplier, frames);
        }

        public override Task OnActivateAsync()
        {
            State = new RenderingOptions();

            return base.OnActivateAsync();
        }
    }
}
