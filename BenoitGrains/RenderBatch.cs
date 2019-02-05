using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    public class RenderBatch<TExport> : Grain, IRenderBatch<TExport>
        where TExport : IConvertible
    {
        public Task<Immutable<TExport[]>> Compute(RenderingOptions options, Complex[] coordinates)
        {
            var bailout = options.BailoutValue * options.BailoutValue;
            var maxIteration = options.MaxIteration;

            var values = new TExport[coordinates.Length];

            for (int n = 0; n < coordinates.Length; n++)
            {
                var i = 0;

                var newVal = Complex.Zero;

                while (newVal.Real * newVal.Real + newVal.Imaginary * newVal.Imaginary <= bailout && i < maxIteration)
                {
                    var tempVal = newVal * newVal + coordinates[n];

                    // Local periodicity checking
                    if (tempVal == newVal)
                    {
                        i = maxIteration - 1;
                    }

                    newVal = tempVal;

                    ++i;
                }

                values[n] = (TExport)Convert.ChangeType(i, typeof(TExport));
            }

            return Task.FromResult(new Immutable<TExport[]>(values));
        }
    }
}