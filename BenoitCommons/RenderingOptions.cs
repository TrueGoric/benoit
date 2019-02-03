using System;

namespace BenoitCommons
{
    public class RenderingOptions
    {
        /// <summary>
        /// Width in pixels of the render frame.
        /// </summary>
        public int FrameWidth { get; set; }

        /// <summary>
        /// Height in pixels of the render frame.
        /// </summary>
        public int FrameHeight { get; set; }

        /// <summary>
        /// Maximum iteration for the escape time algorithm.
        /// </summary>
        public int MaxIteration { get; set; }

        /// <summary>
        /// The absolute value that specifies the divergence threshold.
        /// </summary>
        public int BailoutValue { get; set; }
    }
}
