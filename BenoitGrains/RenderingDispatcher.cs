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
    [Reentrant]
    [StorageProvider(ProviderName = "OptionsStorage")]
    public class RenderingDispatcher<TExport> : Grain<RenderingOptions>, IRenderingDispatcher<TExport>
        where TExport : IConvertible
    {
        #region Private Fields

        private GrainObserverManager<IRenderObserver<TExport>> _observerManager;

        #endregion

        #region IRenderingDispatcher<TExport> implementation

        public Task SetOptions(RenderingOptions options)
        {
            State = options;
            return base.WriteStateAsync();
        }

        public Task<RenderingOptions> GetOptions()
        {            
            return Task.FromResult(State);
        }
        
        public Task<bool> BeginRenderFrame(Guid requestIdentifier, Complex center, double scale, GrainCancellationToken cancellationToken = null)
        {
            DispatchRenderFrame(requestIdentifier, center, scale, cancellationToken);

            return Task.FromResult(true);
        }

        public Task<bool> BeginRenderMovie(Guid requestIdentifier, Complex center, double scale, double scaleMultiplier, int frames, GrainCancellationToken cancellationToken = null)
        {
            DispatchRenderMovie(requestIdentifier, center, scale, scaleMultiplier, frames, cancellationToken);

            return Task.FromResult(true);
        }

        public override Task OnActivateAsync()
        {
            State = new RenderingOptions();

            _observerManager = new GrainObserverManager<IRenderObserver<TExport>>();
            _observerManager.ExpirationDuration = new TimeSpan(1, 0, 0); // 1 hour

            return base.OnActivateAsync();
        }

        public Task Subscribe(IRenderObserver<TExport> observer)
        {
            _observerManager.Subscribe(observer);

            return Task.CompletedTask;
        }

        public Task Unsubscribe(IRenderObserver<TExport> observer)
        {
            _observerManager.Unsubscribe(observer);

            return Task.CompletedTask;
        }

        #endregion

        #region Private Dispatcher Methods

        private async Task DispatchRenderFrame(Guid requestIdentifier, Complex center, double scale, GrainCancellationToken cancellationToken = null)
        {
            var frameRenderer = GrainFactory.GetGrain<IFrameRenderer<TExport>>(Guid.NewGuid());

            var rendered = await frameRenderer.RenderFrame(State, center, scale, cancellationToken);

            _observerManager.Notify(o => o.ReceiveRenderedFrame(requestIdentifier, rendered));
        }

        private async Task DispatchRenderMovie(Guid requestIdentifier, Complex center, double scale, double scaleMultiplier, int frames, GrainCancellationToken cancellationToken = null)
        {
            var movieRenderer = GrainFactory.GetGrain<IMovieRenderer<TExport>>(Guid.NewGuid());
            
            var rendered = await movieRenderer.Render(State, center, scale, scaleMultiplier, frames, cancellationToken);

            _observerManager.Notify(o => o.ReceiveRenderedMovie(requestIdentifier, rendered));
        }

        #endregion
    }
}
