using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    public class FrameRenderer<TExport> : Grain, IFrameRenderer<TExport>
        where TExport : IConvertible
    {
        public async Task<Immutable<Map2D<TExport>>> RenderFrame(RenderingOptions options, Complex center, double scale)
        {
            // Prepare a frame to return
            var frame = new Map2D<TExport>(options.FrameWidth, options.FrameHeight);

            // Load initial values
            var initialSet = PrepareInitialValues(options.FrameWidth, options.FrameHeight, center, scale);

            // Compute in batches
            var batchCount = (int)Math.Ceiling(frame.Raw.Length / (double)options.BatchSize);
            var batchTasks = new Task<Immutable<TExport[]>>[batchCount];

            for (int i = 0; i < batchCount; i++)
            {
                var newBatch = GrainFactory.GetGrain<IRenderBatch<TExport>>(Guid.NewGuid());

                var startPoint = i * options.BatchSize;
                var count = Math.Min(options.BatchSize, frame.Raw.Length - startPoint);

                var buffer = new Span<Complex>(initialSet, startPoint, count).ToArray();

                batchTasks[i] = newBatch.Compute(options, buffer);
            }

            // Wait for the workers to finish
            var combinedTask = Task.WhenAll(batchTasks); // TODO: exception handling
            await combinedTask;

            // Import the results...
            for (int i = 0; i < batchCount; i++)
            {
                var startPoint = i * options.BatchSize;
                
                batchTasks[i].Result.Value.CopyTo(frame.Raw, startPoint);
            }
            
            // ... and send them back to the requestor.
            return new Immutable<Map2D<TExport>>(frame);
        }

        private Complex[] PrepareInitialValues(int pixelWidth, int pixelHeight, Complex center, double scale)
        {
            var initialTable = new Complex[pixelWidth * pixelHeight];

            for (int y = 0; y < pixelHeight; y++)
            {
                for (int x = 0; x < pixelWidth; x++)
                {
                    initialTable[y * pixelWidth + x] = center + new Complex((x - pixelWidth / 2d) * scale, (y - pixelHeight / 2d) * scale);
                }
            }

            return initialTable;
        }
    }
}