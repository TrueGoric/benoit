using System;
using System.Numerics;
using System.Threading.Tasks;
using Orleans;
using BenoitCommons;
using BenoitGrainInterfaces;

namespace BenoitGrains
{
    public class RenderBatch<TExport> : Grain, IRenderBatch<TExport>
        where TExport : IConvertible
    {
        public Task<TExport[]> Compute(RenderingOptions options, Complex[] coordinates)
        {
            var bailout = options.BailoutValue * options.BailoutValue;
            var maxIteration = options.MaxIteration + 1;

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
                        break;
                    }

                    ++i;
                }

                values[n] = (TExport)Convert.ChangeType(i, typeof(TExport));
            }

            return Task.FromResult(values);
        }

        private double Lerp(double first, double second, double by)
        {
            return first * (1 - by) + second * by;
        }
    }
}