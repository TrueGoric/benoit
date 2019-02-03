using System;

namespace BenoitCommons
{
    public class RenderingOptions
    {
        /// <summary>
        /// Width in pixels of the render frame.
        /// </summary>
        public int FrameWidth { get; set; } = 350;

        /// <summary>
        /// Height in pixels of the render frame.
        /// </summary>
        public int FrameHeight { get; set; } = 200;

        /// <summary>
        /// Maximum iteration for the escape time algorithm.
        /// </summary>
        public int MaxIteration { get; set; } = 1000;

        /// <summary>
        /// The absolute value that specifies the divergence threshold from the magnitude.
        /// </summary>
        public int BailoutValue { get; set; } = 1 << 8;

        /// <summary>
        /// Specifies how many pixels should be rendered in a single batch.
        /// </summary>
        public int BatchSize { get; set; } = 100;
    }
}
