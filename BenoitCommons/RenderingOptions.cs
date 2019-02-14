using System;
using Orleans.Concurrency;

namespace BenoitCommons
{
    [Immutable]
    public class RenderingOptions
    {
        /// <summary>
        /// Width in pixels of the render frame.
        /// </summary>
        public int FrameWidth { get; private set; } = 350;

        /// <summary>
        /// Height in pixels of the render frame.
        /// </summary>
        public int FrameHeight { get; private set; } = 200;

        /// <summary>
        /// Maximum iteration for the escape time algorithm.
        /// </summary>
        public int MaxIteration { get; private set; } = 1000;

        /// <summary>
        /// The absolute value that specifies the divergence threshold from the magnitude.
        /// </summary>
        public int BailoutValue { get; private set; } = 1 << 8;

        /// <summary>
        /// Specifies how many pixels should be rendered in a single batch.
        /// </summary>
        public int BatchSize { get; private set; } = 100;

        #region Fluent Setters

        public RenderingOptions WithFrameWidth(int frameWidth)
        {
            var newOptions = (RenderingOptions)this.MemberwiseClone();
            newOptions.FrameWidth = frameWidth;

            return newOptions;
        }

        public RenderingOptions WithFrameHeight(int frameHeight)
        {
            var newOptions = (RenderingOptions)this.MemberwiseClone();
            newOptions.FrameHeight = frameHeight;

            return newOptions;
        }

        public RenderingOptions WithMaxIteration(int maxIteration)
        {
            var newOptions = (RenderingOptions)this.MemberwiseClone();
            newOptions.MaxIteration = maxIteration;

            return newOptions;
        }

        public RenderingOptions WithBailoutValue(int bailoutValue)
        {
            var newOptions = (RenderingOptions)this.MemberwiseClone();
            newOptions.BailoutValue = bailoutValue;

            return newOptions;
        }

        public RenderingOptions WithBatchSize(int batchSize)
        {
            var newOptions = (RenderingOptions)this.MemberwiseClone();
            newOptions.BatchSize = batchSize;

            return newOptions;
        }

        #endregion
    }
}
