using System;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    public class RenderingDispatcher<TExport> : Grain<RenderingOptions>, IRenderingDispatcher<TExport>
        where TExport : IConvertible
    {
        public Task SetOptions(RenderingOptions options)
        {
            State = options;
            return base.WriteStateAsync();
        }

        public Task<RenderingOptions> GetOptions()
        {
            if (State == null)
                SetOptions(new RenderingOptions());
            
            return Task.FromResult(State);
        }

        public async Task<I2DMap<TExport>> RenderFrame(Complex center, double scale)
        {
            throw new NotImplementedException();
        }

        public async Task<I2DMap<TExport>[]> RenderMovie(Complex center, double scale, double scaleMultiplier, int frames)
        {
            throw new NotImplementedException();
        }
    }
}
