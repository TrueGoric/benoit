using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    public class MovieRenderer<TExport> : Grain, IMovieRenderer<TExport>
        where TExport : IConvertible
    {
        public async Task<I2DMap<TExport>[]> Render(RenderingOptions options, Complex center, double scale, double scaleMultiplier, int frames)
        {
            var rendererTasks = new Task<I2DMap<TExport>>[frames];
            var currentScale = scale;

            // Render all frames
            for (int i = 0; i < frames; i++)
            {
                var renderer = GrainFactory.GetGrain<IFrameRenderer<TExport>>(Guid.NewGuid());

                rendererTasks[i] = renderer.RenderFrame(options, center, currentScale);
                currentScale *= scaleMultiplier;
            }

            // Wait for the rendering to finish
            var combinedTask = Task.WhenAll(rendererTasks);
            await combinedTask; // TODO: exception handling

            var readyFrames = new I2DMap<TExport>[frames];

            for (int i = 0; i < frames; i++)
            {
                readyFrames[i] = rendererTasks[i].Result;
            }

            return frames;
        }
    }
}